namespace Ambev.SalesDeveloperEvaluation.Application.Sales.Common
{
    public class SaleResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalDiscounts { get; set; }
        public Guid BranchId { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<SaleItemResult> Items { get; set; } = new List<SaleItemResult>();
    }
}