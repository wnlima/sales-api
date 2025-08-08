using FluentValidation;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
    {
        public GetProductByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");
        }
    }
}
