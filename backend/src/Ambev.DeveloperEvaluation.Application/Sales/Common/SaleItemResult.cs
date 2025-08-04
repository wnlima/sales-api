using Ambev.DeveloperEvaluation.Application.Products.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleItemResult
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public ProductResult Product { get; set; }
    public SaleResult Sale { get; set; }
}