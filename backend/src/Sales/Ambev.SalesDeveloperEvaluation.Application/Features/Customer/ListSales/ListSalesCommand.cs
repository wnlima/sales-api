using Ambev.DeveloperEvaluation.Domain.Common.Filters;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales
{
    public class ListSalesCommand  : AbstractAdvancedFilter, IRequest<ListSalesResult>
    {
        public Guid CustomerId { get; set; }
    }
}