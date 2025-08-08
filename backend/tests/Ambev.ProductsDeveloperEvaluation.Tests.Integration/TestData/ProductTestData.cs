using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.CreateProduct;

using Bogus;

namespace Ambev.ProductsDeveloperEvaluation.Integration.TestData
{
    /// <summary>
    /// Provides methods for generating realistic product data using the Bogus library.
    /// This class ensures consistency in test data creation and offers flexibility
    /// for generating various product scenarios.
    /// </summary>
    public static class ProductTestData
    {
        /// <summary>
        /// Faker instance for generating CreateProductRequest objects with randomized and realistic data.
        /// </summary>
        private static readonly Faker<CreateProductRequest> _createProductFaker = new Faker<CreateProductRequest>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
            .RuleFor(p => p.QuantityInStock, f => f.Random.Int(30, 100));

        /// <summary>
        /// Generates a list of Product objects.
        /// </summary>
        /// <param name="count">The number of products to generate.</param>
        /// <returns>A list of Product objects.</returns>
        public static List<CreateProductRequest> GenerateProducts(int count)
        {
            return _createProductFaker.Generate(count);
        }

        /// <summary>
        /// Generates a single valid CreateProductRequest object.
        /// </summary>
        /// <returns>A CreateProductRequest object with valid data.</returns>
        public static CreateProductRequest GenerateValidRequest()
        {
            return _createProductFaker.Generate();
        }
    }
}