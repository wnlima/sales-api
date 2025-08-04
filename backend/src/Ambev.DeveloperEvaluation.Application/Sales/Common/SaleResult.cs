namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleResult
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalDiscounts { get; set; }
    public string Branch { get; set; }
    public bool IsCancelled { get; set; }
    public ICollection<SaleItemResult> SaleItems { get; set; } = new List<SaleItemResult>();
}