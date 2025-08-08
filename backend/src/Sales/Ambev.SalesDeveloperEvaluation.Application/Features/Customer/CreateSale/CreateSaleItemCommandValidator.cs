using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale
{
    public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
    {
        public CreateSaleItemCommandValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot be greater than 20.");
        }
    }
}