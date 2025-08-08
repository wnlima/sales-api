namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.CancelSale
{
    public class ManagerCancelSaleRequest
    {
        public Guid Id { get; set; }
        public ManagerCancelSaleRequest(Guid id)
        {
            Id = id;
        }
    }
}
