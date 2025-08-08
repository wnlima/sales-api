using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Commands
{
    public class CancelSaleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
    }
}