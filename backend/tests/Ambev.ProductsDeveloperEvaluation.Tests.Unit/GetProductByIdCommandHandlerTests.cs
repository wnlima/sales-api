using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Mappers;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;
using Ambev.ProductsDeveloperEvaluation.TestUtils.TestData;

using AutoMapper;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.ProductsDeveloperEvaluation.Tests.Unit.Application.Products
{
    /// <summary>
    /// Contains unit tests for the <see cref="GetProductByIdCommandHandler"/> class.
    /// </summary>
    public class GetProductByIdCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly GetProductByIdCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProductByIdCommandHandlerTests"/> class.
        /// Sets up the test dependencies using NSubstitute and AutoMapper.
        /// </summary>
        public GetProductByIdCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), loggerFactory);
            _mapper = config.CreateMapper();
            _handler = new GetProductByIdCommandHandler(_productRepository, _mapper);
        }

        /// <summary>
        /// Tests that a product is returned when a valid ID is provided.
        /// </summary>
        [Fact(DisplayName = "Given an existing product ID, when getting a product, then the product is returned")]
        public async Task Handle_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new GetProductByIdCommand(productId);
            var product = ProductTestData.GenerateValidProduct();
            product.Id = productId;
            var resultDto = _mapper.Map<GetProductResult>(product);

            _productRepository.GetByIdAsync(productId).Returns(product);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            await _productRepository.Received(1).GetByIdAsync(productId);
        }

        /// <summary>
        /// Tests that a key not found exception is thrown when the product ID does not exist.
        /// </summary>
        [Fact(DisplayName = "Given a non-existing product ID, when getting a product, then a KeyNotFoundException is thrown")]
        public async Task Handle_NonExistingProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new GetProductByIdCommand(productId);
            _productRepository.GetByIdAsync(productId).Returns((Domain.Entities.Product)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
            await _productRepository.Received(1).GetByIdAsync(productId);
        }

        /// <summary>
        /// Tests that an invalid command throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given an invalid command, when getting a product, then a validation exception is thrown")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var invalidCommand = new GetProductByIdCommand(Guid.Empty);

            // Act
            var act = () => _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}