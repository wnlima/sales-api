using Ambev.UsersDeveloperEvaluation.Application.Users.GetUser;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Unit.Application.Users
{
    public class GetUserProfileTests
    {
        private readonly IMapper _mapper;

        public GetUserProfileTests()
        {
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GetUserProfile>(), loggerFactory);
            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Given User When mapping to GetUserResult Then should map correctly")]
        public void Map_User_To_GetUserResult_ShouldMapCorrectly()
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
            var result = _mapper.Map<GetUserResult>(user);

            // Assert
            result.Should().BeEquivalentTo(new GetUserResult
            {
                Id = user.Id,
                Email = user.Email,
                Phone = user.Phone,
                Status = user.Status,
                Role = user.Role
            });
        }
    }
}