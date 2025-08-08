using System.Net;

using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.UsersDeveloperEvaluation.Tests.Integration.Auxiliary;
using Ambev.UsersDeveloperEvaluation.Tests.Integration.TestData;
using Ambev.UsersDeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.UsersDeveloperEvaluation.WebApi.Features.Users.GetUser;

using Newtonsoft.Json;

using Xunit;

namespace Ambev.UsersDeveloperEvaluation.Tests.Integration.WebApi
{
    public class UsersControllerTests : IAsyncLifetime, IClassFixture<HttpClientFixture>
    {
        private readonly HttpClientFixture _clientFixture;

        public UsersControllerTests(HttpClientFixture clientFixture)
        {
            _clientFixture = clientFixture;
        }


        public Task InitializeAsync()
            => Task.CompletedTask;

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact(DisplayName = "CreateUser - Expect 201 Created on successful user creation")]
        public async Task CreateUser_Expect_201_Created_On_Successful_User_Creation()
        {
            // Arrange
            var data = CreateUserTestData.GenerateCreateUserRequest();
            data.Status = Domain.Enums.UserStatus.Active;
            data.Role = Domain.Enums.UserRole.Customer;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/users", data);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "CreateUser - Expect 400 Bad Request when username is missing")]
        public async Task CreateUser_Expect_400_Bad_Request_When_Username_Is_Missing()
        {
            // Arrange
            var data = CreateUserTestData.GenerateCreateUserRequest();
            data.Username = "";

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/users", data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "GetUser - Expect 200 OK on successful user retrieval")]
        public async Task GetUser_Expect_200_OK_On_Successful_User_Retrieval()
        {
            // Arrange
            // First, create a user to retrieve
            var createData = CreateUserTestData.GenerateCreateUserRequest();
            createData.Status = Domain.Enums.UserStatus.Active;
            createData.Role = Domain.Enums.UserRole.Customer;
            var createResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/users", createData);
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            var createApiResponse = JsonConvert.DeserializeObject<ApiResponseWithData<CreateUserResponse>>(createResponseContent);

            Assert.NotNull(createApiResponse);
            Assert.True(createApiResponse.Success);
            Assert.NotNull(createApiResponse.Data);
            Assert.NotEqual(Guid.Empty, createApiResponse.Data.Id);

            var userId = createApiResponse.Data.Id;

            // Act
            var getResponse = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/users/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getResponseContent = await getResponse.Content.ReadAsStringAsync();
            var getApiResponse = JsonConvert.DeserializeObject<ApiResponseWithData<GetUserResponse>>(getResponseContent)!;

            Assert.NotNull(getApiResponse);
            Assert.True(getApiResponse.Success);
            Assert.NotNull(getApiResponse.Data);
            Assert.Equal(userId, getApiResponse.Data.Id);
        }

        [Fact(DisplayName = "GetUser - Expect 400 Bad Request when user ID is invalid")]
        public async Task GetUser_Expect_400_Bad_Request_When_User_ID_Is_Invalid()
        {
            // Arrange
            var invalidUserId = "invalid-guid";

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/users/{invalidUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "GetUser - Expect 404 Not Found when user ID does not exist")]
        public async Task GetUser_Expect_404_Not_Found_When_User_ID_Does_Not_Exist()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/users/{nonExistentUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "DeleteUser - Expect 200 OK on successful user deletion")]
        public async Task DeleteUser_Expect_200_OK_On_Successful_User_Deletion()
        {
            // Arrange
            var createData = CreateUserTestData.GenerateCreateUserRequest();
            createData.Status = Domain.Enums.UserStatus.Active;
            createData.Role = Domain.Enums.UserRole.Customer;

            var createResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/users", createData);
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            var createApiResponse = JsonConvert.DeserializeObject<ApiResponseWithData<CreateUserResponse>>(createResponseContent);

            Assert.NotNull(createApiResponse);
            Assert.True(createApiResponse.Success);
            Assert.NotNull(createApiResponse.Data);
            Assert.NotEqual(Guid.Empty, createApiResponse.Data.Id);

            var userId = createApiResponse.Data.Id;

            // Act
            var deleteResponse = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/users/{userId}");
            var getResponse = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/users/{userId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);


            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact(DisplayName = "DeleteUser - Expect 400 Bad Request when user ID is invalid")]
        public async Task DeleteUser_Expect_400_Bad_Request_When_User_ID_Is_Invalid()
        {
            // Arrange
            var invalidUserId = "invalid-guid";

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/users/{invalidUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "DeleteUser - Expect 404 Not Found when user ID does not exist")]
        public async Task DeleteUser_Expect_404_Not_Found_When_User_ID_Does_Not_Exist()
        {
            // Arrange
            var nonExistentUserId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/users/{nonExistentUserId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}