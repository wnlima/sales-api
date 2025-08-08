using Ambev.ProductsDeveloperEvaluation.Application.Products.Commands;
using Ambev.ProductsDeveloperEvaluation.Domain.Repositories;

using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var success = await _productRepository.DeleteAsync(request.Id);

            if (!success)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            return true;
        }
    }
}