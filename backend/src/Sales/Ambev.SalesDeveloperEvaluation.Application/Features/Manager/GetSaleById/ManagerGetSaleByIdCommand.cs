using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale
{
    public class ManagerGetSaleByIdCommand : IRequest<ManagerGetSaleResult>
    {
        public Guid Id { get; set; }
    }
}