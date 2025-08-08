using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.ProductsDeveloperEvaluation.Tests.Unit.Application.Products
{
    /// <summary>
    /// Contains unit tests for the <see cref="DeleteProductCommandHandler"/> class.
    /// </summary>
    public class DeleteProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProductCommandHandlerTests"/> class.
        /// Sets up the test dependencies using NSubstitute.
        /// </summary>
        public DeleteProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_productRepository);
        }

        /// <summary>
        /// Tests that deleting an existing product is successful.
        /// </summary>
        [Fact(DisplayName = "Given an existing product ID, when deleting a product, then the operation is successful")]
        public async Task Handle_ExistingProduct_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DeleteProductCommand(productId);
            _productRepository.DeleteAsync(productId).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            await _productRepository.Received(1).DeleteAsync(productId);
        }

        /// <summary>
        /// Tests that deleting a non-existent product throws a key not found exception.
        /// </summary>
        [Fact(DisplayName = "Given a non-existing product ID, when deleting a product, then a KeyNotFoundException is thrown")]
        public async Task Handle_NonExistingProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DeleteProductCommand(productId);
            _productRepository.DeleteAsync(productId).Returns(false);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {productId} not found");
            await _productRepository.Received(1).DeleteAsync(productId);
        }
    }
}