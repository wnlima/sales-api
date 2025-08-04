using System.Net;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Integration.Auxiliary;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Integration.WebApi;

public class SalesControllerTests : IAsyncLifetime, IClassFixture<HttpClientFixture>
{
    private readonly HttpClientFixture _clientFixture;

    public SalesControllerTests(HttpClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
        _clientFixture.BasicDataSeed().GetAwaiter().GetResult();
        _clientFixture.SaleSeed().GetAwaiter().GetResult();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<PaginatedResponse<T>> DeserializePaginatedResponse<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PaginatedResponse<T>>(responseContent)!;
    }

    [Fact(DisplayName = "CreateSale - Expect 201 Created on successful sale creation")]
    public async Task CreateSale_Expect_201_Created_On_Successful_Sale_Creation()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        var product = _clientFixture.DbContext.Products.AsNoTracking().Where(x => x.QuantityInStock >= 20).First();
        var saleItem = new CreateSaleItemRequest();
        saleItem.ProductId = product.Id;
        saleItem.Quantity = 10;
        sale.Items = [saleItem];

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale);
        var apiResponse = await _clientFixture.DeserializeApiResponseWithData<CreateSaleResponse>(response);

        // Assert
        product = _clientFixture.DbContext.Products.AsNoTracking().First(x => x.Id == product.Id);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEqual(Guid.Empty, apiResponse.Data.Id);
        Assert.Equal(sale.Branch, apiResponse.Data.Branch);
        Assert.Equal(product.QuantityInStock, apiResponse.Data.Items.First().Product.QuantityInStock);
        Assert.Equal(saleItem.ProductId, apiResponse.Data.Items.First().Product.Id);
        Assert.Equal(saleItem.Quantity, apiResponse.Data.Items.First().Quantity);
        Assert.Equal(saleItem.Quantity * product.Price * (1 - 0.20m), apiResponse.Data.TotalAmount);
        Assert.Equal(saleItem.Quantity, apiResponse.Data.TotalItems);
        Assert.Equal(sale.SaleDate, apiResponse.Data.SaleDate);
        Assert.Equal(saleItem.Quantity * product.Price * 0.20m, apiResponse.Data.TotalDiscounts);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and default pagination when no parameters are provided")]
    public async Task ListSales_Expect_200_OK_And_Pagination()
    {
        // Act
        for (int i = 0; i < 15; i++)
        {
            var sale = SaleTestData.Generate();
            var product = _clientFixture.DbContext.Products.AsNoTracking().Where(x => x.QuantityInStock >= 1).First();
            var saleItem = new CreateSaleItemRequest();
            saleItem.ProductId = product.Id;
            saleItem.Quantity = 1;
            sale.Items = [saleItem];

            // Act
            var newSaleResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale);
            newSaleResponse.EnsureSuccessStatusCode();
        }

        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales");
        var response2 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_page=2&_size=5");
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
        Assert.Equal(_clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).Count(), apiResponse.AvailableItems);


        Assert.True(apiResponse2.Success);
        Assert.NotNull(apiResponse2);
        Assert.NotNull(apiResponse2.Data);
        Assert.Equal(2, apiResponse2.CurrentPage);
        Assert.Equal(5, apiResponse2.TotalCount);
        Assert.Equal(_clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).Count(), apiResponse2.AvailableItems);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and ordered results when _order desc is provided")]
    public async Task ListSales_Expect_200_OK_And_Desc_Ordered_Results()
    {
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount desc");
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
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount asc");
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
        var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).First();
        string BranchValue = expectd.Branch;

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branch={BranchValue}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        foreach (var sale in apiResponse.Data)
            Assert.Contains(BranchValue, sale.Branch);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and filtered results with start partial match")]
    public async Task ListSales_Expect_200_OK_And_Filtered_Results_Partial_Match()
    {
        // Arrange
        var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).First();
        string partialBranch = expectd.Branch.Substring(0, 5);

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branch={partialBranch}*");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        // Assert
        foreach (var sale in apiResponse.Data)
            Assert.StartsWith(partialBranch, sale.Branch);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and filtered results with end partial match")]
    public async Task ListSales_Expect_200_OK_And_Filtered_Results_Partial_End_Match()
    {
        // Arrange
        var expectd = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).First();
        string partialBranch = expectd.Branch.Substring(expectd.Branch.Length - 5, 5);

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branch=*{partialBranch}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        // Assert
        foreach (var sale in apiResponse.Data)
            Assert.EndsWith(partialBranch, sale.Branch);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and results filtered by price range")]
    public async Task ListSales_Expect_200_OK_And_Results_Filtered_By_TotalAmount_Range()
    {
        // Arrange
        var values = _clientFixture.DbContext.Sales.AsNoTracking().Where(x => x.CustomerId == _clientFixture.CustomerUser.Id).Select(x => x.TotalAmount).Distinct().ToList();

        var random = new Random();
        int minIndex = random.Next(0, values.Count - 1);
        int maxIndex = random.Next(minIndex + 1, values.Count);

        decimal minTotalAmount = values[minIndex];
        decimal maxTotalAmount = values[maxIndex];

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?_minTotalAmount={minTotalAmount}&_maxTotalAmount={maxTotalAmount}");

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
        var response1 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_page=0");
        var response2 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_size=0");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    [Fact(DisplayName = "ListSales - Expect 200 OK and empty list when no results match the filter")]
    public async Task ListSales_Expect_200_OK_And_Empty_List_When_No_Results_Match_Filter()
    {
        // Arrange
        string nonExistentBranch = "NonExistentSale";

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales?branch={nonExistentBranch}");

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
        var firstExpectd = _clientFixture.DbContext.Sales.AsNoTracking().OrderByDescending(p => p.TotalAmount).ThenBy(p => p.Branch).First();
        var lastExpectd = _clientFixture.DbContext.Sales.AsNoTracking().OrderByDescending(p => p.TotalAmount).ThenBy(p => p.Branch).Take(10).Last();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=totalamount desc,saledate asc");
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
        sale.Branch = "";

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "ListSales - Expect 400 OK and ordered results when _order is provided")]
    public async Task ListSales_Expect_400_Bad_Request_Ordered_Results()
    {
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/sales?_order=price desc");
        var apiResponse = await DeserializePaginatedResponse<SaleResponse>(response);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "GetSaleById - Expect 200 OK on successful sale retrieval")]
    public async Task GetSaleById_Expect_200_OK_On_Successful_Sale_Retrieval()
    {
        // Arrange
        var sale = _clientFixture.DbContext.Sales.AsNoTracking().First();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales/{sale.Id}");
        var apiResponse = await _clientFixture.DeserializeApiResponseWithData<GetSaleResponse>(response);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(sale.Id, apiResponse.Data.Id);
        Assert.Equal(sale.Branch, apiResponse.Data.Branch);
    }

    [Fact(DisplayName = "GetSaleById - Expect 404 Not Found when sale ID does not exist")]
    public async Task GetSaleById_Expect_404_Not_Found_When_Sale_ID_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/sales/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "CancelSale - Expect 204 NoContent on successful sale ")]
    public async Task CancelSale_Expect_204_NoContent_On_Successful_Sale_Deletion()
    {
        // Arrange
        var sale = SaleTestData.Generate();
        var product = _clientFixture.DbContext.Products.AsNoTracking().Where(x => x.QuantityInStock >= 20).First();
        var saleItem = new CreateSaleItemRequest();
        saleItem.ProductId = product.Id;
        saleItem.Quantity = 1;
        sale.Items = [saleItem];

        // Act
        var createResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/sales", sale);
        var createdSale = await _clientFixture.DeserializeApiResponseWithData<SaleResponse>(createResponse);

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{createdSale.Data.Id}", _clientFixture.ManagerUser.Token);
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
        var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{nonExistentId}", _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "CancelSale - Expect 404 Forbidden invalid token")]
    public async Task CancelSale_Expect_403_Invalid_Role()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/sales/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}