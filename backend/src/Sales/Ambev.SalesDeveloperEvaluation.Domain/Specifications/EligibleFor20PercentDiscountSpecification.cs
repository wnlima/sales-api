using Ambev.SalesDeveloperEvaluation.Domain.Entities;

namespace Ambev.SalesDeveloperEvaluation.Domain.Specifications
{
    public class EligibleFor20PercentDiscountSpecification : ISpecification<SaleItem>
    {
        public bool IsSatisfiedBy(SaleItem item)
        {
            return item.Quantity >= 10 && item.Quantity <= 20;
        }
    }
}
