using Ambev.UsersDeveloperEvaluation.Application.Auth.AuthenticateUser;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Auth
{
    public class AuthenticateUserValidatorTests
    {
        private readonly AuthenticateUserValidator _validator;

        public AuthenticateUserValidatorTests()
        {
            _validator = new AuthenticateUserValidator();
        }

        [Fact(DisplayName = "Given valid request When validating Then should not have any errors")]
        public async Task Validate_ValidRequest_HasNoErrors()
        {
            // Arrange
            var request = new AuthenticateUserCommand
            {
                Email = "test@example.com",
                Password = "password"
            };

            // Act
            var result = await _validator.TestValidateAsync(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Given empty email When validating Then should have error for Email")]
        public async Task Validate_EmptyEmail_HasErrorForEmail()
        {
            // Arrange
            var request = new AuthenticateUserCommand
            {
                Email = "",
                Password = "password"
            };

            // Act
            var result = await _validator.TestValidateAsync(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact(DisplayName = "Given invalid email When validating Then should have error for Email")]
        public async Task Validate_InvalidEmail_HasErrorForEmail()
        {
            // Arrange
            var request = new AuthenticateUserCommand
            {
                Email = "invalid-email",
                Password = "password"
            };

            // Act
            var result = await _validator.TestValidateAsync(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact(DisplayName = "Given empty password When validating Then should have error for Password")]
        public async Task Validate_EmptyPassword_HasErrorForPassword()
        {
            // Arrange
            var request = new AuthenticateUserCommand
            {
                Email = "test@example.com",
                Password = ""
            };

            // Act
            var result = await _validator.TestValidateAsync(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}