using Ambev.SalesDeveloperEvaluation.WebApi.Features.Sales.Common;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale
{
    public class ManagerGetSaleResponse : SaleResponse
    {
        public Guid CustomerId { get; set; }
    }
}