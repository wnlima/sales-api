using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Validators;

public class ListSalesRequestValidator : AbstractAdvancedFilterValidator<ListSalesResponse, ListSalesRequest>
{
    public ListSalesRequestValidator() : base()
    {
        RuleFor(x => x.CustomerId)
        .NotEmpty().WithMessage("Customer is required.");
    }
}