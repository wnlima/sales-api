using Ambev.UsersDeveloperEvaluation.Application.Users.GetUser;

using FluentValidation.TestHelper;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users
{
    public class GetUserCommandValidatorTests
    {
        private readonly GetUserValidator _validator;

        public GetUserCommandValidatorTests()
        {
            _validator = new GetUserValidator();
        }

        [Fact(DisplayName = "Given valid id When validating Then should not have any errors")]
        public async Task Validate_ValidId_HasNoErrors()
        {
            // Arrange
            var command = new GetUserCommand(Guid.NewGuid());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Given empty id When validating Then should have error for Id")]
        public async Task Validate_EmptyId_HasErrorForId()
        {
            // Arrange
            var command = new GetUserCommand(Guid.Empty);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}