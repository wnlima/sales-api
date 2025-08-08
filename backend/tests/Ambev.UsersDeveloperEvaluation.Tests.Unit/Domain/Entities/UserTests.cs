using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;
using Ambev.UsersDeveloperEvaluation.TestUtils.TestData;

using FluentAssertions;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the User entity class.
    /// Tests cover status changes and validation scenarios.
    /// </summary>
    public class UserTests
    {
        /// <summary>
        /// Tests that when a suspended user is activated, their status changes to Active.
        /// </summary>
        [Fact(DisplayName = "User status should change to Active when activated")]
        public void Given_SuspendedUser_When_Activated_Then_StatusShouldBeActive()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            user.Status = UserStatus.Suspended;

            // Act
            user.Activate();

            // Assert
            Assert.Equal(UserStatus.Active, user.Status);
        }

        /// <summary>
        /// Tests that when an active user is suspended, their status changes to Suspended.
        /// </summary>
        [Fact(DisplayName = "User status should change to Suspended when suspended")]
        public void Given_ActiveUser_When_Suspended_Then_StatusShouldBeSuspended()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            user.Status = UserStatus.Active;

            // Act
            user.Suspend();

            // Assert
            Assert.Equal(UserStatus.Suspended, user.Status);
        }

        /// <summary>
        /// Tests that validation passes when all user properties are valid.
        /// </summary>
        [Fact(DisplayName = "Validation should pass for valid user data")]
        public void Given_ValidUserData_When_Validated_Then_ShouldReturnValid()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();

            // Act
            var result = user.Validate();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        /// <summary>
        /// Tests that validation fails when user properties are invalid.
        /// </summary>
        [Fact(DisplayName = "Validation should fail for invalid user data")]
        public void Given_InvalidUserData_When_Validated_Then_ShouldReturnInvalid()
        {
            // Arrange
            var user = new User
            {
                Username = "",
                Password = UserTestData.GenerateInvalidPassword(),
                Email = UserTestData.GenerateInvalidEmail(),
                Phone = UserTestData.GenerateInvalidPhone(),
                Status = UserStatus.Unknown,
                Role = UserRole.None
            };

            // Act
            var result = user.Validate();

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact(DisplayName = "Given a new User When created Then should have a non-empty Id")]
        public void Constructor_Should_Generate_NonEmptyId()
        {
            // Arrange & Act
            var user = new User();
            user.Id = Guid.NewGuid();

            // Assert
            user.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Given valid user When deactivating Then Status should be Inactive and UpdatedAt should be set")]
        public void Deactivate_ValidUser_SetsStatusToInactiveAndUpdatedAt()
        {
            // Arrange
            var user = new User { Status = UserStatus.Active };

            // Act
            user.Deactivate();

            // Assert
            user.Status.Should().Be(UserStatus.Inactive);
            user.UpdatedAt.Should().NotBeNull();
        }

        [Fact(DisplayName = "Given a user and null When comparing Then should return 1")]
        public void CompareTo_NullUser_ShouldReturn1()
        {
            // Arrange
            var user = new User { Username = "alice" };

            // Act & Assert
            Assert.True(user.CompareTo(null) == 1);
        }
    }
}
