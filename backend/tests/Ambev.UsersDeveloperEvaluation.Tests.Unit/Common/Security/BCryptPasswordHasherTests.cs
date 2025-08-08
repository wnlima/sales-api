using Ambev.DeveloperEvaluation.Domain.Common.Security;

using FluentAssertions;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Common.Security
{
    public class BCryptPasswordHasherTests
    {
        private readonly BCryptPasswordHasher _passwordHasher;

        public BCryptPasswordHasherTests()
        {
            _passwordHasher = new BCryptPasswordHasher();
        }

        [Fact(DisplayName = "Given a password When hashing Then should return a non-empty hash")]
        public void HashPassword_Should_Return_NonEmptyHash()
        {
            // Arrange
            string password = "testPassword123";

            // Act
            string hash = _passwordHasher.HashPassword(password);

            // Assert
            hash.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Given a password and a hash When verifying a correct password Then should return true")]
        public void VerifyPassword_CorrectPassword_Should_Return_True()
        {
            // Arrange
            string password = "testPassword123";
            string hash = _passwordHasher.HashPassword(password);

            // Act
            bool result = _passwordHasher.VerifyPassword(password, hash);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Given a password and a hash When verifying an incorrect password Then should return false")]
        public void VerifyPassword_IncorrectPassword_Should_Return_False()
        {
            // Arrange
            string password = "testPassword123";
            string hash = _passwordHasher.HashPassword(password);
            string incorrectPassword = "wrongPassword";

            // Act
            bool result = _passwordHasher.VerifyPassword(incorrectPassword, hash);

            // Assert
            Assert.False(result);
        }
    }
}