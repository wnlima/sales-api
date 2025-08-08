using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Commands
{
    public class ManagerCancelSaleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
