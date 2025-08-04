using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalDiscounts { get; set; }
    public string Branch { get; set; }
    public bool IsCancelled { get; set; }
    public ICollection<CreateSaleItemCommand> SaleItems { get; set; }
}