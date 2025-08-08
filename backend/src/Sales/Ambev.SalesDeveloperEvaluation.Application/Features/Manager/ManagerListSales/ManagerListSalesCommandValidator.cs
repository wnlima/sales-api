using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.ListSales
{
    public class ManagerListSalesCommandValidator : AbstractAdvancedFilterValidator<Sale, ManagerListSalesCommand> { }
}