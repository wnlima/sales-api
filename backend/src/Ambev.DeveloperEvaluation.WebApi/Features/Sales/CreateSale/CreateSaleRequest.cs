namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public Guid CustomerId { get; set; }
    public DateTime SaleDate { get; set; }
    public string Branch { get; set; }
    public ICollection<CreateSaleItemRequest> Items { get; set; }
}