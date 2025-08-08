using Ambev.DeveloperEvaluation.Domain.Common.Filters;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsRequest : AbstractAdvancedFilter
    {
        public ListProductsRequest(Dictionary<string, string>? filters)
        {
            Filters = filters;
            ResolveFields();
        }
    }
}
