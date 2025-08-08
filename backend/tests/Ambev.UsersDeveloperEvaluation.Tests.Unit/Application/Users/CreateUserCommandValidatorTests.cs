using Ambev.UsersDeveloperEvaluation.Application.Users.CreateUser;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserCommandValidatorTests()
        {
            _validator = new CreateUserCommandValidator();
        }

        [Fact(DisplayName = "Given valid command When validating Then should not have any errors")]
        public async Task Validate_ValidCommand_HasNoErrors()
        {
            // Arrange
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
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Given empty username When validating Then should have error for Username")]
        public async Task Validate_EmptyUsername_HasErrorForUsername()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact(DisplayName = "Given invalid email When validating Then should have error for Email")]
        public async Task Validate_InvalidEmail_HasErrorForEmail()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "invalid-email",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact(DisplayName = "Given short username When validating Then should have error for Username")]
        public async Task Validate_ShortUsername_HasErrorForUsername()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "ab",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact(DisplayName = "Given long username When validating Then should have error for Username")]
        public async Task Validate_LongUsername_HasErrorForUsername()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "ThisIsAVeryLongUsernameThatExceedsTheMaximumLength!",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact(DisplayName = "Given invalid password When validating Then should have error for Password")]
        public async Task Validate_InvalidPassword_HasErrorForPassword()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "weak",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact(DisplayName = "Given invalid phone When validating Then should have error for Phone")]
        public async Task Validate_InvalidPhone_HasErrorForPhone()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "a123456789",
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Fact(DisplayName = "Given unknown status When validating Then should have error for Status")]
        public async Task Validate_UnknownStatus_HasErrorForStatus()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Unknown,
                Role = UserRole.Admin
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact(DisplayName = "Given none role When validating Then should have error for Role")]
        public async Task Validate_NoneRole_HasErrorForRole()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.None
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }
    }
}