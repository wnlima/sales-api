using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Manager.GetSale
{
    public class ManagerGetSaleByIdRequestValidator : AbstractValidator<ManagerGetSaleByIdRequest>
    {
        public ManagerGetSaleByIdRequestValidator() : base()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");
        }
    }
}
