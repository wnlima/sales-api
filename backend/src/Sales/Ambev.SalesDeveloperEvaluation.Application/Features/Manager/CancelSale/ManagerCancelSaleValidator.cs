using Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Commands;

using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.Validators
{
    public class ManagerCancelSaleValidator : AbstractValidator<ManagerCancelSaleCommand>
    {
        public ManagerCancelSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}