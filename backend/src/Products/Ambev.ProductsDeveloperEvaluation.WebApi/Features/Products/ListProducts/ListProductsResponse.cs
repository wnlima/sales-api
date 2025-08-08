using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.ListProducts
{
    public class ListProductsResponse : PaginatedList<GetProductResponse> { }
}
