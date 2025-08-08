using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;
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

namespace Ambev.ProductsDeveloperEvaluation.Unit.Application.Products
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateProductCommandHandler"/> class.
    /// </summary>
    public class CreateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly CreateProductCommandHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductCommandHandlerTests"/> class.
        /// Sets up the test dependencies using NSubstitute.
        /// </summary>
        public CreateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            var loggerFactory = Substitute.For<Microsoft.Extensions.Logging.ILoggerFactory>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), loggerFactory);
            _mapper = config.CreateMapper();
            _handler = new CreateProductCommandHandler(_productRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid product creation request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given a valid command, when creating a product, then a success response is returned")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = ProductTestData.GenerateCreateProductCommand();
            var product = _mapper.Map<Product>(command);
            var createdProduct = _mapper.Map<Product>(command);
            createdProduct.Id = Guid.NewGuid();

            var resultDto = _mapper.Map<CreateProductResult>(createdProduct);

            _productRepository.CreateAsync(Arg.Any<Product>()).Returns(createdProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdProduct.Id);
            await _productRepository.Received(1).CreateAsync(Arg.Is<Product>(p => p.Name == command.Name));
        }

        /// <summary>
        /// Tests that an invalid product creation request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given an invalid command, when creating a product, then a validation exception is thrown")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var invalidCommand = new CreateProductCommand { Name = "", Description = "descrição", Price = 100 }; // Invalid command

            // Act
            var act = () => _handler.Handle(invalidCommand, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}