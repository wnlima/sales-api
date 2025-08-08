using System.Net;

using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Common.Security;
using Ambev.SalesDeveloperEvaluation.Integration.Auxiliary;
using Ambev.SalesDeveloperEvaluation.Tests.Integration.TestData;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.ListSales;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.UsersDeveloperEvaluation.Domain.Entities;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using NSubstitute;

using Xunit;

namespace Ambev.SalesDeveloperEvaluation.Tests.Integration.WebApi
{
    public class SalesControllerTests : IAsyncLifetime, IClassFixture<HttpClientFixture>
    {
        private readonly HttpClientFixture _clientFixture;
        private Guid _invaliManagerId;
        private string _invaliManagerToken;
        private Guid _valiManagerId;
        private string _valiManagerToken;
        private Guid _valiCustomerId;
        private string _valiCustomerToken;
        private Guid _valiAdminId;
        private string _valiAdminToken;

        public SalesControllerTests(HttpClientFixture clientFixture)
        {
            _clientFixture = clientFixture;
            JwtTokenGeneratorTests();
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => Task.CompletedTask;

        void JwtTokenGeneratorTests()
        {
            var validTokenGenerator = new JwtTokenGenerator(_clientFixture.Configuration);
            var configuration = Substitute.For<IConfiguration>();

            configuration["Jwt:SecretKey"].Returns("SuperInvalidSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong");
            var invalidTokenGenerator = new JwtTokenGenerator(configuration);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                Role = UserRole.Manager,
                Status = UserStatus.Active
            };

            _invaliManagerId = user.Id;
            _invaliManagerToken = invalidTokenGenerator.GenerateToken(user);

            user.Id = Guid.NewGuid();
            _valiManagerId = user.Id;
            _valiManagerToken = validTokenGenerator.GenerateToken(user);

            user.Role = UserRole.Customer;
            user.Id = Guid.NewGuid();
            _valiCustomerId = user.Id;
            _valiCustomerToken = validTokenGenerator.GenerateToken(user);

            user.Role = UserRole.Admin;
            user.Id = Guid.NewGuid();
            _valiAdminId = user.Id;
            _valiAdminToken = validTokenGenerator.GenerateToken(user);
        }

        private async Task<PaginatedResponse<T>> DeserializePaginatedResponse<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginatedResponse<T>>(responseContent)!;
        }

        [Fact(DisplayName = "CreateSale - Expect 201 Created on successful sale creation with discount")]
        public async Task CreateSale_Expect_201_Created_On_Successful_Sale_Creation()
        {
            // Arrange
            var sale = SaleTestData.Generate();
            var saleItem = SaleTestData.ItemGenerate();
            saleItem.Quantity = 10;
            sale.Items = [saleItem];
            var dtStart = DateTime.UtcNow;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
            var apiResponse = await _clientFixture.DeserializeApiResponseWithData<CreateSaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.NotEqual(Guid.Empty, apiResponse.Data.Id);
            Assert.Equal(sale.BranchId, apiResponse.Data.BranchId);
            Assert.Equal(saleItem.ProductId, apiResponse.Data.Items.First().ProductId);
            Assert.Equal(saleItem.Quantity, apiResponse.Data.Items.First().Quantity);
            Assert.Equal(saleItem.Quantity * saleItem.UnitPrice * (1 - 0.20m), apiResponse.Data.TotalAmount);
            Assert.Equal(saleItem.Quantity, apiResponse.Data.TotalItems);
            Assert.True(apiResponse.Data.SaleDate >= dtStart);
            Assert.Equal(saleItem.Quantity * saleItem.UnitPrice * 0.20m, apiResponse.Data.TotalDiscounts);
        }

        [Fact(DisplayName = "CreateSale - Expect 201 Created on successful sale creation without discount")]
        public async Task CreateSale_Expect_201_Created_On_Successful_Sale_Creation_without_discount()
        {
            // Arrange
            var sale = SaleTestData.Generate();
            var saleItem = SaleTestData.ItemGenerate();
            saleItem.Quantity = 3;
            sale.Items = [saleItem];
            var dtStart = DateTime.UtcNow;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
            var apiResponse = await _clientFixture.DeserializeApiResponseWithData<CreateSaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.NotEqual(Guid.Empty, apiResponse.Data.Id);
            Assert.Equal(sale.BranchId, apiResponse.Data.BranchId);
            Assert.Equal(saleItem.ProductId, apiResponse.Data.Items.First().ProductId);
            Assert.Equal(saleItem.Quantity, apiResponse.Data.Items.First().Quantity);
            Assert.Equal(saleItem.Quantity * saleItem.UnitPrice, apiResponse.Data.TotalAmount);
            Assert.Equal(saleItem.Quantity, apiResponse.Data.TotalItems);
            Assert.True(apiResponse.Data.SaleDate >= dtStart);
            Assert.Equal(0, apiResponse.Data.TotalDiscounts);
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and default pagination when no parameters are provided")]
        public async Task ListSales_Expect_200_OK_And_Pagination()
        {
            // Act
            for (int i = 0; i < 15; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales", _valiCustomerToken);
            var response2 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_page=2&_size=5", _valiCustomerToken);
            var apiResponse = await DeserializePaginatedResponse<ListSalesResponse>(response);
            var apiResponse2 = await DeserializePaginatedResponse<ListSalesResponse>(response2);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal(1, apiResponse.CurrentPage);
            Assert.Equal(10, apiResponse.TotalCount);

            Assert.True(apiResponse2.Success);
            Assert.NotNull(apiResponse2);
            Assert.NotNull(apiResponse2.Data);
            Assert.Equal(2, apiResponse2.CurrentPage);
            Assert.Equal(5, apiResponse2.TotalCount);
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and ordered results when _order desc is provided")]
        public async Task ListSales_Expect_200_OK_And_Desc_Ordered_Results()
        {
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount desc", _valiCustomerToken);
            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            var currentVale = apiResponse.Data.First().TotalAmount;
            foreach (var sale in apiResponse.Data)
            {
                Assert.True(sale.TotalAmount <= currentVale);
                currentVale = sale.TotalAmount;
            }
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and ordered results when _order asc is provided")]
        public async Task ListSales_Expect_200_OK_And_Asc_Ordered_Results()
        {
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount asc", _valiCustomerToken);
            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            var currentVale = apiResponse.Data.First().TotalAmount;
            foreach (var sale in apiResponse.Data)
            {
                Assert.True(sale.TotalAmount >= currentVale);
                currentVale = sale.TotalAmount;
            }
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and filtered results when a filter is provided")]
        public async Task ListSales_Expect_200_OK_And_Filtered_Results()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _valiCustomerId).First();
            Guid BranchValue = expectd.BranchId;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branchid={BranchValue}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            foreach (var sale in apiResponse.Data)
                Assert.Equal(BranchValue, sale.BranchId);
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and filtered results with start partial match")]
        public async Task ListSales_Expect_200_OK_And_Filtered_Results_Partial_Match()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _valiCustomerId).First();
            var bId = expectd.BranchId.ToString();
            string partialBranchId = bId.Substring(0, 5);

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branchid={partialBranchId}*", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            // Assert
            foreach (var sale in apiResponse.Data)
                Assert.StartsWith(partialBranchId, sale.BranchId.ToString());
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and filtered results with end partial match")]
        public async Task ListSales_Expect_200_OK_And_Filtered_Results_Partial_End_Match()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _valiCustomerId).First();
            var id = expectd.BranchId.ToString();
            string partialBranchId = id.Substring(id.Length - 5, 5);

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branchid=*{partialBranchId}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            // Assert
            foreach (var sale in apiResponse.Data)
                Assert.EndsWith(partialBranchId, sale.BranchId.ToString());
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and results filtered by price range")]
        public async Task ListSales_Expect_200_OK_And_Results_Filtered_By_TotalAmount_Range()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var sale = SaleTestData.Generate();
                var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
                newSaleResponse.EnsureSuccessStatusCode();
            }

            var values = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _valiCustomerId).Select(x => x.TotalAmount).Distinct().ToList();

            var random = new Random();
            int minIndex = random.Next(0, values.Count - 1);
            int maxIndex = random.Next(minIndex + 1, values.Count);

            decimal minTotalAmount = values[minIndex];
            decimal maxTotalAmount = values[maxIndex];

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?_minTotalAmount={minTotalAmount}&_maxTotalAmount={maxTotalAmount}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);

            foreach (var sale in apiResponse.Data)
                Assert.True(sale.TotalAmount >= minTotalAmount && sale.TotalAmount <= maxTotalAmount);
        }

        [Fact(DisplayName = "ListSales - Expect 400 Bad Request when _page or _size is invalid")]
        public async Task ListSales_Expect_400_Bad_Request_When_Page_Or_Size_Is_Invalid()
        {
            // Act
            var response1 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_page=0", _valiCustomerToken);
            var response2 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_size=0", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and empty list when no results match the filter")]
        public async Task ListSales_Expect_200_OK_And_Empty_List_When_No_Results_Match_Filter()
        {
            // Arrange
            Guid nonExistentBranchId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branchid={nonExistentBranchId}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.Empty(apiResponse.Data);
        }

        [Fact(DisplayName = "ListSales - Expect 200 OK and results ordered by multiple fields")]
        public async Task ListSales_Expect_200_OK_And_Results_Ordered_By_Multiple_Fields()
        {
            // Arrange
            var firstExpectd = _clientFixture.DbContext.Sales.AsNoTracking().OrderByDescending(p => p.TotalAmount).ThenBy(p => p.BranchId).First();
            var lastExpectd = _clientFixture.DbContext.Sales.AsNoTracking().OrderByDescending(p => p.TotalAmount).ThenBy(p => p.BranchId).Take(10).Last();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount desc,saledate asc", _valiCustomerToken);
            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
        }

        [Fact(DisplayName = "CreateSale - Expect 400 Bad Request when sale name is missing")]
        public async Task CreateSale_Expect_400_Bad_Request_When_Branch_Is_Missing()
        {
            // Arrange
            var sale = SaleTestData.Generate();
            sale.BranchId = Guid.Empty;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "ListSales - Expect 400 OK and ordered results when _order is provided")]
        public async Task ListSales_Expect_400_Bad_Request_Ordered_Results()
        {
            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=price desc", _valiCustomerToken);
            var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "GetSaleById - Expect 200 OK on successful sale retrieval")]
        public async Task GetSaleById_Expect_200_OK_On_Successful_Sale_Retrieval()
        {
            // Arrange
            var s = SaleTestData.Generate();
            var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", s, _valiCustomerToken);
            newSaleResponse.EnsureSuccessStatusCode();
            var sale = (await _clientFixture.DeserializeApiResponseWithData<SaleResponse>(newSaleResponse)).Data;

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales/{sale.Id}", _valiCustomerToken);
            var apiResponse = await _clientFixture.DeserializeApiResponseWithData<GetSaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal(sale.Id, apiResponse.Data.Id);
            Assert.Equal(sale.BranchId, apiResponse.Data.BranchId);
        }

        [Fact(DisplayName = "GetSaleById - Expect 404 Not Found when sale ID does not exist")]
        public async Task GetSaleById_Expect_404_Not_Found_When_Sale_ID_Does_Not_Exist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales/{nonExistentId}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "CancelSale - Expect 204 NoContent on successful sale ")]
        public async Task CancelSale_Expect_204_NoContent_On_Successful_Sale_Deletion()
        {
            // Arrange
            var sale = SaleTestData.Generate();

            // Act
            var createResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale, _valiCustomerToken);
            var createdSale = await _clientFixture.DeserializeApiResponseWithData<SaleResponse>(createResponse);

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{createdSale.Data.Id}", _valiCustomerToken);
            var dataResponse = await _clientFixture.DeserializeApiResponseWithData<SaleResponse>(response);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "CancelSale - Expect 404 Not Found when sale ID does not exist")]
        public async Task CancelSale_Expect_404_Not_Found_When_Sale_ID_Does_Not_Exist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{nonExistentId}", _valiManagerToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "CancelSale - Expect 404 Not Found invalid token")]
        public async Task CancelSale_Expect_404_Invalid_Role()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{nonExistentId}", _valiCustomerToken);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "CancelSale - Expect 401 invalid token")]
        public async Task CancelSale_Expect_401_Invalid_Role()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{nonExistentId}", _invaliManagerToken);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}