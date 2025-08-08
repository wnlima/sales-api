using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(command => command.BranchId)
                .NotEmpty().WithMessage("BranchId is required.");

            RuleFor(command => command.Items)
                .NotEmpty().WithMessage("Sale must have at least one item.")
                .Must(HaveValidQuantities).WithMessage("Invalid quantity in sale items.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleForEach(command => command.Items)
                .SetValidator(new CreateSaleItemCommandValidator());
        }

        private bool HaveValidQuantities(ICollection<CreateSaleItemCommand> items)
        {
            return items.All(item => item.Quantity > 0 && item.Quantity <= 20);
        }
    }
}
