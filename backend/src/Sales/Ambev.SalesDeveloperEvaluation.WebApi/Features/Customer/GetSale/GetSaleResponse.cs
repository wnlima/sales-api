using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale
{
    public class GetSaleResponse : SaleResponse
    {
        public Guid CustomerId { get; set; }
    }
}