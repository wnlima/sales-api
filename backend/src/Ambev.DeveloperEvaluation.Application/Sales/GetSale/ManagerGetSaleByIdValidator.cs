using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class ManagerGetSaleByIdValidator : AbstractValidator<ManagerGetSaleByIdCommand>
{
    public ManagerGetSaleByIdValidator()
    {

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}