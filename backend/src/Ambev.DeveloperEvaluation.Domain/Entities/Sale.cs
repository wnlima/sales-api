using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale in the system.
/// This  follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseUserIdentity
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalDiscounts { get; set; }
    public string Branch { get; set; }
    public bool IsCancelled { get; set; }
    public User Customer { get; set; }
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public void Compute()
    {
        TotalAmount = SaleItems.Sum(item => item.TotalAmount);
        TotalDiscounts = SaleItems.Sum(item => item.Discount * item.UnitPrice * item.Quantity);
        TotalItems = SaleItems.Sum(item => item.Quantity);
    }
}