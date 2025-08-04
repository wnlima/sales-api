using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class ManagerGetSaleByIdRequest : IIdentifier
{
    public Guid Id { get; set; }

    public ManagerGetSaleByIdRequest(Guid id)
    {
        Id = id;
    }
}