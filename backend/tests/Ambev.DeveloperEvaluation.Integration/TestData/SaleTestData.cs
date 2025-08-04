using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

/// <summary>
/// Provides methods for generating realistic sale data using the Bogus library.
/// This class ensures consistency in test data creation and offers flexibility
/// for generating various sale scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Faker instance for generating CreateSaleRequest objects with randomized and realistic data.
    /// </summary>
    private static readonly Faker<CreateSaleRequest> _faker = new Faker<CreateSaleRequest>()
        .RuleFor(s => s.SaleDate, f => f.Date.Past(1)) // dentro do último ano
        .RuleFor(s => s.Branch, f => f.Company.CompanyName());

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
    /// Generates a single valid CreateSaleRequest object.
    /// </summary>
    /// <returns>A CreateSaleRequest object with valid data.</returns>
    public static CreateSaleRequest Generate()
    {
        return _faker.Generate();
    }
}