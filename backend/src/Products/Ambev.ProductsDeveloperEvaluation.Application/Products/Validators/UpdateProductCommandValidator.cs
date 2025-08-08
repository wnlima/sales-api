using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;

using FluentValidation;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty();

            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(p => p.Description)
                .MaximumLength(1000);

            RuleFor(p => p.Price)
                .GreaterThan(0);

            RuleFor(p => p.QuantityInStock)
                .GreaterThanOrEqualTo(0);
        }
    }

}
