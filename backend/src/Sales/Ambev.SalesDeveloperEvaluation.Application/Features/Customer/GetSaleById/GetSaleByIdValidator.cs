using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.GetSale
{
    public class GetSaleByIdValidator : AbstractValidator<GetSaleByIdCommand>
    {
        public GetSaleByIdValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer is required.");

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");

            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required.");
        }
    }
}