namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale
{
    public class CreateSaleItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}