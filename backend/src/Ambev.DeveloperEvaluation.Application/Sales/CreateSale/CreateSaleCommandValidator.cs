using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(command => command.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

        RuleFor(command => command.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Sale date cannot be in the future.");

        RuleFor(command => command.Branch)
            .NotEmpty().WithMessage("Branch is required.")
            .MaximumLength(255).WithMessage("Branch cannot exceed 255 characters.");

        RuleFor(command => command.SaleItems)
            .NotEmpty().WithMessage("Sale must have at least one item.")
            .Must(HaveValidQuantities).WithMessage("Invalid quantity in sale items.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required.");

        RuleForEach(command => command.SaleItems)
            .SetValidator(new CreateSaleItemCommandValidator());
    }

    private bool HaveValidQuantities(ICollection<CreateSaleItemCommand> items)
    {
        return items.All(item => item.Quantity > 0 && item.Quantity <= 20);
    }
}
