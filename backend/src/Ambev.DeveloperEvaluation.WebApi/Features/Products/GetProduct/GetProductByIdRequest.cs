using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

public class GetProductByIdRequest
{
    public GetProductByIdRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
