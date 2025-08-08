using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.DTOs;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.Validators
{
    public class ManagerListSalesRequestValidator : AbstractAdvancedFilterValidator<Sale, ManagerListSalesRequest>
    {
        public ManagerListSalesRequestValidator() : base()
        {
        }
    }
}