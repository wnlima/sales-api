using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.Common
{
    public class SaleResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalDiscounts { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<SaleItemResponse> Items { get; set; } = new List<SaleItemResponse>();
    }
}
