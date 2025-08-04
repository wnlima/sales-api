using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class ManagerCreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    public ManagerCreateSaleItemCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot be greater than 20.");
    }
}