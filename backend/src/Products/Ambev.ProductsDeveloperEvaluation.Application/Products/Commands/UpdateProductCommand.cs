using Ambev.ProductsDeveloperEvaluation.Application.Products.DTOs;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<UpdateProductResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}