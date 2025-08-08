using Ambev.DeveloperEvaluation.Domain.Common.Security;
using Ambev.UsersDeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;
using Ambev.UsersDeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Auth
{
    public class AuthenticateUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly AuthenticateUserHandler _handler;

        public AuthenticateUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
             var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthenticateUserProfile>(), loggerFactory);
            _mapper = config.CreateMapper();

            _handler = new AuthenticateUserHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
        }

        [Fact(DisplayName = "Given inactive user When authenticating Then should throw UnauthorizedAccessException")]
        public async Task Handle_InactiveUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var command = new AuthenticateUserCommand { Email = "test@example.com", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Email = command.Email, Password = "hashedPassword", Status = UserStatus.Inactive };

            _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));
            _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact(DisplayName = "Given valid credentials When authenticating Then should return authentication result with token")]
        public async Task Handle_ValidCredentials_ReturnsAuthResultWithToken()
        {
            // Arrange
            var command = new AuthenticateUserCommand { Email = "test@example.com", Password = "password" };
            var user = new User { Id = Guid.NewGuid(), Email = command.Email, Password = "hashedPassword", Status = UserStatus.Active };
            var result = new AuthenticateUserResult { Token = "token" };

            _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));
            _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);
            _jwtTokenGenerator.GenerateToken(user).Returns("token");

            // Act
            var authResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            authResult.Should().NotBeNull();
            authResult.Token.Should().Be("token");
        }

        [Fact(DisplayName = "Given invalid email When authenticating Then should throw UnauthorizedAccessException")]
        public async Task Handle_InvalidEmail_ThrowsNotFoundException()
        {
            // Arrange
            var command = new AuthenticateUserCommand { Email = "invalid@example.com", Password = "password" };

            _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(null));

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid credentials");
        }

        [Fact(DisplayName = "Given invalid password When authenticating Then should throw UnauthorizedAccessException")]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var command = new AuthenticateUserCommand { Email = "test@example.com", Password = "wrongPassword" };
            var user = new User { Id = Guid.NewGuid(), Email = command.Email, Password = "hashedPassword" };

            _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<User?>(user));
            _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(false);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}