using System.Net;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.DTOs;
using Newtonsoft.Json;
using Ambev.DeveloperEvaluation.Application.Products.Commands;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Integration.Auxiliary;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

namespace Ambev.DeveloperEvaluation.Integration.WebApi;

public class ProductsControllerTests : IAsyncLifetime, IClassFixture<HttpClientFixture>
{
    private readonly HttpClientFixture _clientFixture;

    public ProductsControllerTests(HttpClientFixture clientFixture)
    {
        _clientFixture = clientFixture;
        _clientFixture.BasicDataSeed().GetAwaiter().GetResult();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<PaginatedResponse<T>> DeserializePaginatedResponse<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PaginatedResponse<T>>(responseContent)!;
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and default pagination when no parameters are provided")]
    public async Task ListProducts_Expect_200_OK_And_Default_Pagination()
    {
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(1, apiResponse.CurrentPage);
        Assert.Equal(10, apiResponse.TotalCount);
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and correct pagination when _page and _size are provided")]
    public async Task ListProducts_Expect_200_OK_And_Correct_Pagination()
    {
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products?_page=2&_size=5");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(2, apiResponse.CurrentPage);
        Assert.Equal(5, apiResponse.TotalCount);
        Assert.Equal(_clientFixture.DbContext.Products.AsNoTracking().Count(), apiResponse.AvailableItems);
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and ordered results when _order is provided")]
    public async Task ListProducts_Expect_200_OK_And_Ordered_Results()
    {
        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products?_order=price desc");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        decimal previousPrice = decimal.MaxValue;
        foreach (var product in apiResponse.Data)
        {
            Assert.True(product.Price <= previousPrice);
            previousPrice = product.Price;
        }
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and filtered results when a filter is provided")]
    public async Task ListProducts_Expect_200_OK_And_Filtered_Results()
    {
        // Arrange
        var expectd = _clientFixture.DbContext.Products.AsNoTracking().First();
        string filterValue = expectd.Name;

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products?name={filterValue}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        foreach (var product in apiResponse.Data)
        {
            Assert.Contains(filterValue, product.Name);
        }
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and filtered results with partial match")]
    public async Task ListProducts_Expect_200_OK_And_Filtered_Results_Partial_Match()
    {
        // Arrange
        var expectd = _clientFixture.DbContext.Products.AsNoTracking().First();
        string partialName = expectd.Name.Substring(0, 5);

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products?name={partialName}*");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        // Assert that all products have the partial name (adapt based on your data)
        foreach (var product in apiResponse.Data)
        {
            Assert.StartsWith(partialName, product.Name);
        }
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and results filtered by price range")]
    public async Task ListProducts_Expect_200_OK_And_Results_Filtered_By_Price_Range()
    {
        // Arrange
        decimal minPrice = 10;
        decimal maxPrice = 20;

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products?_minPrice={minPrice}&_maxPrice={maxPrice}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        foreach (var product in apiResponse.Data)
        {
            Assert.True(product.Price >= minPrice && product.Price <= maxPrice);
        }
    }

    [Fact(DisplayName = "ListProducts - Expect 400 Bad Request when _page or _size is invalid")]
    public async Task ListProducts_Expect_400_Bad_Request_When_Page_Or_Size_Is_Invalid()
    {
        // Act
        var response1 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products?_page=0");
        var response2 = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products?_size=0");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response1.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and empty list when no results match the filter")]
    public async Task ListProducts_Expect_200_OK_And_Empty_List_When_No_Results_Match_Filter()
    {
        // Arrange
        string nonExistentName = "NonExistentProduct";

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products?name={nonExistentName}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Empty(apiResponse.Data);
    }

    [Fact(DisplayName = "ListProducts - Expect 200 OK and results ordered by multiple fields")]
    public async Task ListProducts_Expect_200_OK_And_Results_Ordered_By_Multiple_Fields()
    {
        // Arrange
        var firstExpectd = _clientFixture.DbContext.Products.AsNoTracking().OrderByDescending(p => p.Price).ThenBy(p => p.Name).First();
        var lastExpectd = _clientFixture.DbContext.Products.AsNoTracking().OrderByDescending(p => p.Price).ThenBy(p => p.Name).Take(10).Last();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, "/api/products?_order=price desc,name asc");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await DeserializePaginatedResponse<ListProductsResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);

        Assert.Equal(firstExpectd.Name, apiResponse.Data.First().Name);
        Assert.Equal(firstExpectd.Price, apiResponse.Data.First().Price);
        Assert.Equal(lastExpectd.Name, apiResponse.Data.Last().Name);
        Assert.Equal(lastExpectd.Price, apiResponse.Data.Last().Price);
    }


    [Fact(DisplayName = "CreateProduct - Expect 201 Created on successful product creation")]
    public async Task CreateProduct_Expect_201_Created_On_Successful_Product_Creation()
    {
        // Arrange
        var product = ProductTestData.GenerateValidRequest();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/products", product, _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var apiResponse = await _clientFixture.DeserializeApiResponseWithData<CreateProductResponse>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.NotEqual(Guid.Empty, apiResponse.Data.Id);
        Assert.Equal(product.Name, apiResponse.Data.Name);
        Assert.Equal(product.Description, apiResponse.Data.Description);
        Assert.Equal(product.Price, apiResponse.Data.Price);
        Assert.Equal(product.QuantityInStock, apiResponse.Data.QuantityInStock);
    }

    [Fact(DisplayName = "CreateProduct - Expect 400 Bad Request when product name is missing")]
    public async Task CreateProduct_Expect_400_Bad_Request_When_Name_Is_Missing()
    {
        // Arrange
        var product = ProductTestData.GenerateValidRequest();
        product.Name = "";

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Post, "/api/products", product, _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "GetProductById - Expect 200 OK on successful product retrieval")]
    public async Task GetProductById_Expect_200_OK_On_Successful_Product_Retrieval()
    {
        // Arrange
        var product = _clientFixture.DbContext.Products.AsNoTracking().First();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products/{product.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await _clientFixture.DeserializeApiResponseWithData<ProductResult>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(product.Id, apiResponse.Data.Id);
        Assert.Equal(product.Name, apiResponse.Data.Name);
    }

    [Fact(DisplayName = "GetProductById - Expect 404 Not Found when product ID does not exist")]
    public async Task GetProductById_Expect_404_Not_Found_When_Product_ID_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Get, $"/api/products/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "UpdateProduct - Expect 200 OK on successful product update")]
    public async Task UpdateProduct_Expect_200_OK_On_Successful_Product_Update()
    {
        // Arrange
        var product = _clientFixture.DbContext.Products.AsNoTracking().First();

        var updatedProduct = new UpdateProductCommand
        {
            Id = product.Id,
            Name = "Updated Product Name",
            Description = "Updated Description",
            Price = 99.99m,
            QuantityInStock = 50
        };

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Put, $"/api/products/{product.Id}", updatedProduct, _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var apiResponse = await _clientFixture.DeserializeApiResponseWithData<ProductResult>(response);

        Assert.NotNull(apiResponse);
        Assert.True(apiResponse.Success);
        Assert.NotNull(apiResponse.Data);
        Assert.Equal(updatedProduct.Name, apiResponse.Data.Name);
        Assert.Equal(updatedProduct.Description, apiResponse.Data.Description);
        Assert.Equal(updatedProduct.Price, apiResponse.Data.Price);
        Assert.Equal(updatedProduct.QuantityInStock, apiResponse.Data.QuantityInStock);
    }

    [Fact(DisplayName = "UpdateProduct - Expect 400 Bad Request when product Name is missing")]
    public async Task UpdateProduct_Expect_400_Bad_Request_When_Id_Mismatch()
    {
        // Arrange
        var updatedProduct = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Description = "Updated Description",
            Price = 99.99m,
            QuantityInStock = 50
        };

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Put, $"/api/products/{Guid.NewGuid()}", updatedProduct, _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "UpdateProduct - Expect 404 Not Found when product ID does not exist")]
    public async Task UpdateProduct_Expect_404_Not_Found_When_Product_ID_Does_Not_Exist()
    {
        // Arrange
        var updatedProduct = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Description = "Updated Description",
            Price = 99.99m,
            QuantityInStock = 50
        };

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Put, $"/api/products/{Guid.NewGuid()}", updatedProduct, _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "DeleteProduct - Expect 204 NoContent on successful product deletion")]
    public async Task DeleteProduct_Expect_204_NoContent_On_Successful_Product_Deletion()
    {
        // Arrange
        var product = ProductTestData.GenerateValidRequest();
        var createResponse = await _clientFixture.RequestSend(HttpMethod.Post, "/api/products", product, _clientFixture.ManagerUser.Token);
        createResponse.EnsureSuccessStatusCode();
        var createdProduct = await _clientFixture.DeserializeApiResponseWithData<ProductResult>(createResponse);

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/products/{createdProduct.Data.Id}", _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(DisplayName = "DeleteProduct - Expect 404 Not Found when product ID does not exist")]
    public async Task DeleteProduct_Expect_404_Not_Found_When_Product_ID_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _clientFixture.RequestSend(HttpMethod.Delete, $"/api/products/{nonExistentId}", _clientFixture.ManagerUser.Token);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}