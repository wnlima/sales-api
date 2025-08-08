using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;

namespace Ambev.ProductsDeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity, IIdentifier
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}