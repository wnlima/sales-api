using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validators;

public class ManagerCancelSaleValidator : AbstractValidator<ManagerCancelSaleCommand>
{
    public ManagerCancelSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}