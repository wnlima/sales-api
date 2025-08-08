using Ambev.DeveloperEvaluation.Domain.Common.Filters;

using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales
{
    public class ManagerListSalesCommand : AbstractAdvancedFilter, IRequest<ManagerListSalesResult>
    {
    }
}