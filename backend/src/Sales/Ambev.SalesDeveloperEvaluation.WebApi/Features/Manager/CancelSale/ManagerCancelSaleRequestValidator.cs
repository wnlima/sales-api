using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.CancelSale
{
    public class ManagerCancelSaleRequestValidator : AbstractValidator<ManagerCancelSaleRequest>
    {
        public ManagerCancelSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");
        }
    }
}