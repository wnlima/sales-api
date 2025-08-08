using Ambev.DeveloperEvaluation.Domain.Common.Filters;
using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Commands
{
    public class ListProductsCommand : AbstractAdvancedFilter, IRequest<ListProductsResult>
    {
    }
}