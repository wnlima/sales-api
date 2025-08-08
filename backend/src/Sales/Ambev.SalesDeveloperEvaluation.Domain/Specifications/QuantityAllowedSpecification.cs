using Ambev.SalesDeveloperEvaluation.Domain.Entities;

namespace Ambev.SalesDeveloperEvaluation.Domain.Specifications
{
    public class QuantityAllowedSpecification : ISpecification<SaleItem>
    {
        public bool IsSatisfiedBy(SaleItem item)
        {
            return item.Quantity >= 1 && item.Quantity <= 20;
        }
    }
}
