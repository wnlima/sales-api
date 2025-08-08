using Ambev.DeveloperEvaluation.Domain.Common.Security;
using Ambev.UsersDeveloperEvaluation.Application.Users.CreateUser;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;
using Ambev.UsersDeveloperEvaluation.Domain.Repositories;
using Ambev.UsersDeveloperEvaluation.TestUtils.TestData;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateUserHandler"/> class.
    /// </summary>
    public class CreateUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly CreateUserHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
        /// Sets up the test dependencies and creates fake data generators.
        /// </summary>
        public CreateUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateUserProfile>(), loggerFactory);
            _mapper = config.CreateMapper();

            _passwordHasher = new BCryptPasswordHasher();
            _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
        }

        /// <summary>
        /// Tests that a valid user creation request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = CreateUserHandlerTestData.GenerateCreateUserCommand();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Password = command.Password,
                Email = command.Email,
                Phone = command.Phone,
                Status = command.Status,
                Role = command.Role
            };

            var result = new CreateUserResult
            {
                Id = user.Id,
            };

            _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns(user);

            // When
            var createUserResult = await _handler.Handle(command, CancellationToken.None);

            // Then
            createUserResult.Should().NotBeNull();
            createUserResult.Id.Should().Be(user.Id);
            await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid user creation request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Given
            var command = new CreateUserCommand(); // Empty command will fail validation

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        /// <summary>
        /// Tests that the password is hashed before saving the user.
        /// </summary>
        [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
        public async Task Handle_ValidRequest_HashesPassword()
        {
            // Given
            var command = CreateUserHandlerTestData.GenerateCreateUserCommand();
            var originalPassword = command.Password;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Password = command.Password,
                Email = command.Email,
                Phone = command.Phone,
                Status = command.Status,
                Role = command.Role
            };

            _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns(user);

            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            await _userRepository.Received(1).CreateAsync(
                Arg.Is<User>(u => u.Password != originalPassword),
                Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given existing email When creating user Then throws exception")]
        public async Task Handle_ExistingEmail_ThrowsException()
        {
            // Arrange
            var command = CreateUserHandlerTestData.GenerateCreateUserCommand();
            var existingUser = new User { Email = command.Email };

            _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(existingUser));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"User with email {command.Email} already exists");
        }

        [Fact]
        public void CreateUserCommand_To_User_Mapping_Should_Be_Valid()
        {
            // Arrange
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateUserProfile>(), loggerFactory);
            var mapper = config.CreateMapper();

            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var user = mapper.Map<User>(command);

            // Assert
            user.Should().BeEquivalentTo(new User
            {
                Username = "testuser",
                Email = "test@example.com",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            }, options => options.Excluding(o => o.CreatedAt)
                                 .Excluding(o => o.Password));
        }
    }
}
