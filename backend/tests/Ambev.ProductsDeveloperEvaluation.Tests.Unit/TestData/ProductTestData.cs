using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

using Bogus;

namespace Ambev.ProductsDeveloperEvaluation.TestUtils.TestData
{
    /// <summary>
    /// Provides methods for generating realistic product data using the Bogus library.
    /// This class ensures consistency in test data creation and offers flexibility
    /// for generating various product scenarios.
    /// </summary>
    public static class ProductTestData
    {
        /// <summary>
        /// Faker instance for generating Product objects with randomized and realistic data.
        /// </summary>
        private static readonly Faker<Product> _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
            .RuleFor(p => p.QuantityInStock, f => f.Random.Int(30, 100));

        /// <summary>
        /// Faker instance for generating CreateProductCommand objects.
        /// </summary>
        private static readonly Faker<CreateProductCommand> _createProductCommandFaker = new Faker<CreateProductCommand>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000));

        /// <summary>
        /// Generates a list of Product objects.
        /// </summary>
        /// <param name="count">The number of products to generate.</param>
        /// <returns>A list of Product objects.</returns>
        public static List<Product> GenerateProducts(int count)
        {
            return _productFaker.Generate(count);
        }

        /// <summary>
        /// Generates a single valid Product object.
        /// </summary>
        /// <returns>A Product object with valid data.</returns>
        public static Product GenerateValidProduct()
        {
            return _productFaker.Generate();
        }

        /// <summary>
        /// Generates a single valid CreateProductCommand object.
        /// </summary>
        /// <returns>A CreateProductCommand object with valid data.</returns>
        public static CreateProductCommand GenerateCreateProductCommand()
        {
            return _createProductCommandFaker.Generate();
        }

        /// <summary>
        /// Generates a valid UpdateProductCommand object.
        /// </summary>
        public static UpdateProductCommand GenerateUpdateProductCommand(Guid id)
        {
            var faker = new Faker<UpdateProductCommand>()
                .RuleFor(p => p.Id, id)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000));
            return faker.Generate();
        }
    }
}