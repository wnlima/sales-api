using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale;

using Bogus;

namespace Ambev.SalesDeveloperEvaluation.TestUtils.TestData
{
    /// <summary>
    /// Provides methods for generating realistic sale data using the Bogus library.
    /// This class ensures consistency in test data creation and offers flexibility
    /// for generating various sale scenarios.
    /// </summary>
    public static class SaleTestData
    {
        /// <summary>
        /// Faker instance for generating CreateSaleCommand objects with randomized and realistic data.
        /// </summary>
        private static readonly Faker<CreateSaleRequest> _faker = new Faker<CreateSaleRequest>()
            .RuleFor(s => s.BranchId, f => Guid.NewGuid())
            .RuleFor(s => s.Items, f => f.Make(3, () => new CreateSaleItemRequest
            {
                ProductId = Guid.NewGuid(),
                Quantity = f.Random.Int(1, 20),
                UnitPrice = f.Finance.Amount(1, 1000, 2),
            }));

        /// <summary>
        /// Generates a list of Sale objects.
        /// </summary>
        /// <param name="count">The number of sales to generate.</param>
        /// <returns>A list of Sale objects.</returns>
        public static List<CreateSaleRequest> Generate(int count)
        {
            return _faker.Generate(count);
        }

        /// <summary>
        /// Generates a single valid CreateSaleCommand object.
        /// </summary>
        /// <returns>A CreateSaleCommand object with valid data.</returns>
        public static CreateSaleRequest Generate()
        {
            return _faker.Generate();
        }
    }
}