namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CreateSale
{
    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}