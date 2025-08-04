using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(p => p.Description)
            .MaximumLength(1000);

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0."); ;

        RuleFor(p => p.QuantityInStock)
            .GreaterThanOrEqualTo(0);
    }
}
