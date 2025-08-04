namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;
public class GetSaleByIdCommand : ManagerGetSaleByIdCommand
{
    public Guid CustomerId { get; set; }
}