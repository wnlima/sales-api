using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsRequest : AbstractAdvancedFilter
{
    public ListProductsRequest(Dictionary<string, string>? filters)
    {
        Filters = filters;
        ResolveFields();
    }
}
