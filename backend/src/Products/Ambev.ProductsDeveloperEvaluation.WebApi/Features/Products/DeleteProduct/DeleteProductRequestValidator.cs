using FluentValidation;

namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.DeleteProduct
{
    public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Product ID is required");
        }
    }
}