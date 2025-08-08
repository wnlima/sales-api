using Ambev.UsersDeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using AutoMapper;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserProfileTests
    {
        private readonly IMapper _mapper;

        public AuthenticateUserProfileTests()
        {
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthenticateUserProfile>(), loggerFactory);
            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Given User When mapping to AuthenticateUserResult Then should map correctly")]
        public void Map_User_To_AuthenticateUserResult_ShouldMapCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                Phone = "+1234567890",
                Status = UserStatus.Active,
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _mapper.Map<AuthenticateUserResult>(user);

            // Assert
            Assert.Equal(user.Id, result.Id);
        }
    }
}