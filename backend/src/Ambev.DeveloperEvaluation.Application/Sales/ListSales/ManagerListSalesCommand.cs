using Ambev.DeveloperEvaluation.Domain.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ManagerListSalesCommand : AbstractAdvancedFilter, IRequest<ListSalesResult>
{
}