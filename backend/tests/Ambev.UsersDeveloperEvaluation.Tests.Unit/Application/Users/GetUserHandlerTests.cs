using Ambev.UsersDeveloperEvaluation.Application.Users.GetUser;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;
using Ambev.UsersDeveloperEvaluation.Domain.Repositories;
using Ambev.UsersDeveloperEvaluation.TestUtils.TestData;

using AutoMapper;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users
{
    public class GetUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly GetUserHandler _handler;
        private readonly IValidator<GetUserCommand> _validator;

        public GetUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GetUserProfile>(), loggerFactory);
            _mapper = config.CreateMapper();

            _handler = new GetUserHandler(_userRepository, _mapper);
            _validator = Substitute.For<IValidator<GetUserCommand>>();
        }

        [Fact(DisplayName = "Given invalid command When getting user Then should throw ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new GetUserCommand(Guid.Empty);
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Id", "Id must not be empty.")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validationResult));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given valid id When getting user Then should return user result")]
        public async Task Handle_ValidId_ReturnsUserResult()
        {
            // Arrange
            var command = new GetUserCommand(Guid.NewGuid());
            var user = UserTestData.GenerateValidUser();

            _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));

            // Act
            var getUserResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            getUserResult.Should().NotBeNull();
            getUserResult.Id.Should().Be(user.Id);
        }

        [Fact(DisplayName = "Given invalid id When getting user Then should throw NotFoundException")]
        public async Task Handle_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var command = new GetUserCommand(Guid.NewGuid());

            _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(null));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact(DisplayName = "Given valid id When getting user Then should return user result with correct Role")]
        public async Task Handle_ValidId_ReturnsUserResultWithCorrectRole()
        {
            // Arrange
            var command = new GetUserCommand(Guid.NewGuid());
            var user = UserTestData.GenerateValidUser();
            user.Role = UserRole.Admin;

            _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));

            // Act
            var getUserResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            getUserResult.Should().NotBeNull();
            getUserResult.Role.Should().Be(user.Role);
        }

        [Fact(DisplayName = "Given valid id When getting user Then should return user result with correct Status")]
        public async Task Handle_ValidId_ReturnsUserResultWithCorrectStatus()
        {
            // Arrange
            var command = new GetUserCommand(Guid.NewGuid());
            var user = UserTestData.GenerateValidUser();
            user.Status = UserStatus.Inactive;

            _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));

            // Act
            var getUserResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            getUserResult.Should().NotBeNull();
            getUserResult.Status.Should().Be(user.Status);
        }
    }
}