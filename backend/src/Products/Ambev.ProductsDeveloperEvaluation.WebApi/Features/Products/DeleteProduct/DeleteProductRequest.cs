namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.DeleteProduct
{
    public class DeleteProductRequest
    {
        public Guid Id { get; set; }

        public DeleteProductRequest(Guid id)
        {
            Id = id;
        }
    }
}
