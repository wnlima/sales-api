namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class ManagerCancelSaleRequest
{
    public Guid Id { get; set; }

    public ManagerCancelSaleRequest(Guid id)
    {
        Id = id;
    }
}
