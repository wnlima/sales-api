namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.CancelSale
{
    public class CancelSaleRequest
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; }

        public CancelSaleRequest(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }
    }
}
