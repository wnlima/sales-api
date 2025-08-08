using MediatR;

namespace Ambev.ProductsDeveloperEvaluation.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }
}
