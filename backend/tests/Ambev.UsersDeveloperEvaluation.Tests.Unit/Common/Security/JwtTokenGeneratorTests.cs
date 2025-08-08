using System.IdentityModel.Tokens.Jwt;

using Ambev.DeveloperEvaluation.WebApi.Common.Security;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Common.Security
{
    public class JwtTokenGeneratorTests
    {
        private const string SECRET_KEY = "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong";

        [Fact(DisplayName = "Given a user When generating token Then should return a valid token")]
        public void GenerateToken_Should_Return_ValidToken()
        {
            // Arrange
            var _configuration = Substitute.For<IConfiguration>();
            _configuration["Jwt:SecretKey"].Returns(SECRET_KEY);
            var _tokenGenerator = new JwtTokenGenerator(_configuration);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                Role = UserRole.Admin,
                Status = UserStatus.Active
            };

            // Act
            string token = _tokenGenerator.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(SECRET_KEY));
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            Assert.Contains(jwtToken.Claims, x => x.Value == user.Id.ToString());
            Assert.Contains(jwtToken.Claims, x => x.Value == user.Role.ToString());
        }
    }
}