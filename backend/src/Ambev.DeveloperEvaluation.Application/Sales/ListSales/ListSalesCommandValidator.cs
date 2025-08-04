using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesCommandValidator : AbstractAdvancedFilterValidator<ListSalesResult, ListSalesCommand>
{
    public ListSalesCommandValidator() : base()
    {
        RuleFor(x => x.CustomerId)
        .NotEmpty().WithMessage("Customer is required.");
    }
}

public class ManagerListSalesCommandValidator : AbstractAdvancedFilterValidator<ListSalesResult, ListSalesCommand> { }