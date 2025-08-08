using FluentValidation;

namespace Ambev.SalesDeveloperEvaluation.WebApi.Features.Customer.GetSale
{
    public class GetSaleByIdRequestValidator : AbstractValidator<GetSaleByIdRequest>
    {
        public Guid CustomerId { get; set; }
        public GetSaleByIdRequestValidator() : base()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");

            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required");
        }
    }
}
