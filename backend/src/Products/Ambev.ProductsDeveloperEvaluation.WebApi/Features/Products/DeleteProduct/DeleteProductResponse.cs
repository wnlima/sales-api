namespace Ambev.ProductsDeveloperEvaluation.WebApi.Features.Products.DeleteProduct
{
    public class DeleteProductResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}