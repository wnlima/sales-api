namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale
{
    public class CreateSaleRequest
    {
        public Guid BranchId { get; set; }
        public ICollection<CreateSaleItemRequest> Items { get; set; }
    }
}