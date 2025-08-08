using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale
{
    public class GetSaleByIdRequest : ManagerGetSaleByIdRequest
    {
        public Guid CustomerId { get; set; }
        public GetSaleByIdRequest(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
        }
    }
}
