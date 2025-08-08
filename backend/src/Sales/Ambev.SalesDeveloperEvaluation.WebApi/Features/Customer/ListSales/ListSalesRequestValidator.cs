using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;
using Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.DTOs;

using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.Validators
{
    public class ListSalesRequestValidator : AbstractAdvancedFilterValidator<Sale, ListSalesRequest>
    {
        public ListSalesRequestValidator() : base()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required.");
        }
    }
}