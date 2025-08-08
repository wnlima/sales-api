using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

namespace Ambev.SalesDeveloperEvaluation.Domain.Specifications
{
    public class EligibleFor10PercentDiscountSpecification : ISpecification<SaleItem>
    {
        public bool IsSatisfiedBy(SaleItem item)
        {
            return item.Quantity >= 4 && item.Quantity <= 9;
        }
    }
    public class ProductAlreadyExistsSpecification : ISpecification<Sale>
    {
        private readonly Guid _productId;

        public ProductAlreadyExistsSpecification(Guid productId)
        {
            _productId = productId;
        }

        public bool IsSatisfiedBy(Sale sale)
        {
            return sale.Items.Any(i => i.ProductId == _productId);
        }
    }
}