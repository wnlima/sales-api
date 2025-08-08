using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Manager.GetSale
{
    public class ManagerGetSaleByIdValidator : AbstractValidator<ManagerGetSaleByIdCommand>
    {
        public ManagerGetSaleByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}