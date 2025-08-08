using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Commands
{
    public class GetProductByIdCommand : IRequest<GetProductResult>
    {
        public Guid Id { get; set; }

        public GetProductByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}