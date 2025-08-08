using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.ListSales
{
    public class ListSalesCommandValidator : AbstractAdvancedFilterValidator<Sale, ListSalesCommand>
    {
        public ListSalesCommandValidator() : base()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required.");
        }
    }
}