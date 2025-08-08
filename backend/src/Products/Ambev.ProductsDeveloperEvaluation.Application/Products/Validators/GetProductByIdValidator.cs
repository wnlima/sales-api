using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;

using FluentValidation;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Validators
{
    public class GetProductByIdValidator : AbstractValidator<GetProductByIdCommand>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}