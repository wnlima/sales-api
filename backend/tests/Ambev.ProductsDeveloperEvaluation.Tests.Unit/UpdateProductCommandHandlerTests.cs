using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Mappers;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;
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
    /// Contains unit tests for the <see cref="UpdateProductCommandHandler"/> class.
    /// </summary>
    public class UpdateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly UpdateProductCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductCommandHandlerTests"/> class.
        /// Sets up the test dependencies using NSubstitute and AutoMapper.
        /// </summary>
        public UpdateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), loggerFactory);
            _mapper = config.CreateMapper();
            _handler = new UpdateProductCommandHandler(_productRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid product update request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given a valid command and an existing product, when updating a product, then the product is successfully updated")]
        public async Task Handle_ExistingProduct_ReturnsSuccessResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updateCommand = ProductTestData.GenerateUpdateProductCommand(productId);
            var existingProduct = ProductTestData.GenerateValidProduct();
            existingProduct.Id = productId;

            var updatedProduct = _mapper.Map<Product>(updateCommand);
            updatedProduct.Id = productId;

            _productRepository.GetByIdAsync(productId).Returns(existingProduct);
            _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            await _productRepository.Received(1).GetByIdAsync(productId);
            await _productRepository.Received(1).UpdateAsync(Arg.Is<Product>(p => p.Id == productId && p.Name == updateCommand.Name));
        }

        /// <summary>
        /// Tests that an update request for a non-existent product throws a key not found exception.
        /// </summary>
        [Fact(DisplayName = "Given a non-existing product ID, when updating a product, then a KeyNotFoundException is thrown")]
        public async Task Handle_NonExistingProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updateCommand = ProductTestData.GenerateUpdateProductCommand(productId);
            _productRepository.GetByIdAsync(productId).Returns((Domain.Entities.Product)null);

            // Act
            var act = () => _handler.Handle(updateCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
            await _productRepository.Received(1).GetByIdAsync(productId);
            await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>());
        }

        /// <summary>
        /// Tests that an invalid update request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given an invalid command, when updating a product, then a validation exception is thrown")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var invalidCommand = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "", Description = "descrição", Price = 100 }; // Invalid command

            // Act
            var act = () => _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}