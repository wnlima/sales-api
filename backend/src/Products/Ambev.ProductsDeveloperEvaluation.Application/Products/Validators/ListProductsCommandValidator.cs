using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Validators
{
    public class ListProductsCommandValidator : AbstractAdvancedFilterValidator<Product, ListProductsCommand>
    {
    }
}