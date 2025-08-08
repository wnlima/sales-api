using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale
{
    public class GetSaleByIdCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
    }
}