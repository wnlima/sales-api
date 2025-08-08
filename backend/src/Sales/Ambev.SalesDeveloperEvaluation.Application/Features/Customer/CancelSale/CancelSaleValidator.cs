using Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Commands;

using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.Validators
{
    public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
    {
        public CancelSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required");
        }
    }
}