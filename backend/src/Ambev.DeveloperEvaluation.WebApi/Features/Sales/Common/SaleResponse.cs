using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public class SaleResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public string Branch { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalDiscounts { get; set; }
    public bool IsCancelled { get; set; }
    public ICollection<SaleItemResponse> Items { get; set; } = new List<SaleItemResponse>();
}
