namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleByIdRequest : ManagerGetSaleByIdRequest
{
    public Guid CustomerId { get; set; }
    public GetSaleByIdRequest(Guid id, Guid customerId) : base(id)
    {
        Id = id;
        CustomerId = customerId;
    }
}
