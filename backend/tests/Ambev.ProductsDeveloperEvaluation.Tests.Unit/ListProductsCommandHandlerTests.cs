using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Mappers;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;
using Ambev.ProductsDeveloperEvaluation.TestUtils.TestData;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.ListProducts;

using AutoMapper;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.ProductsDeveloperEvaluation.Tests.Unit.Application.Products
{
    /// <summary>
    /// Contains unit tests for the <see cref="ListProductsCommandHandler"/> class.
    /// </summary>
    public class ListProductsCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ListProductsCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListProductsCommandHandlerTests"/> class.
        /// Sets up the test dependencies using NSubstitute and AutoMapper.
        /// </summary>
        public ListProductsCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<WebApi.Mappings.ProductProfile>();
            }, loggerFactory);
            _mapper = config.CreateMapper();
            _handler = new ListProductsCommandHandler(_productRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid list command returns a paginated list of products.
        /// </summary>
        [Fact(DisplayName = "Given a valid list command, when listing products, then a paginated list is returned")]
        public async Task Handle_ValidCommand_ReturnsPaginatedList()
        {
            // Arrange
            var filters = new Dictionary<string, string>
            {
                { "_page", "1" },
                { "_size", "10" },
                { "_order", "name asc" }
            };

            var request = new ListProductsRequest(filters);
            var command = _mapper.Map<ListProductsCommand>(request);

            var products = ProductTestData.GenerateProducts(10);
            var paginatedList = new PaginatedList<Product>(products, 1, 10, 1);
            var resultDto = _mapper.Map<ListProductsResult>(paginatedList);

            _productRepository.ListAsync(Arg.Is<ListProductsCommand>(c => c.PageNumber == 1)).Returns(paginatedList);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(paginatedList.TotalCount);
            result.Data.Count().Should().Be(products.Count);
            await _productRepository.Received(1).ListAsync(Arg.Is<ListProductsCommand>(c => c.PageNumber == 1));
        }

        /// <summary>
        /// Tests that an invalid list command throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given an invalid list command, when listing products, then a validation exception is thrown")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var filters = new Dictionary<string, string>
            {
                { "_page", "0" },
                { "_size", "10" },
                { "_order", "name asc" }
            };

            var request = new ListProductsRequest(filters);
            var invalidCommand = _mapper.Map<ListProductsCommand>(request);

            // Act
            var act = () => _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}