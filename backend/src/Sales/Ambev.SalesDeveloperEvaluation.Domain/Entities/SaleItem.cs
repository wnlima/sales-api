using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.Specifications;

namespace Ambev.SalesDeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        private SaleItem() { }

        public static SaleItem Create(Guid saleId,Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new DomainException("Item quantity must be greater than zero.");
            if (unitPrice <= 0) throw new DomainException("Item price must be greater than zero.");
            if (productId == Guid.Empty) throw new DomainException("Product ID cannot be empty.");

            var item = new SaleItem
            {
                Id = Guid.NewGuid(),
                SaleId = saleId,
                IsCancelled = false,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice
            };

            if (!new QuantityAllowedSpecification().IsSatisfiedBy(item))
                throw new InvalidOperationException("Item quantity is not allowed.");

            item.ApplyDiscount();

            item.TotalAmount = (item.UnitPrice * item.Quantity) - item.Discount;

            return item;
        }

        private void ApplyDiscount()
        {
            this.Discount = 0m;

            if (new EligibleFor20PercentDiscountSpecification().IsSatisfiedBy(this))
                this.Discount = this.UnitPrice * this.Quantity * 0.20m;
            else if (new EligibleFor10PercentDiscountSpecification().IsSatisfiedBy(this))
                this.Discount = this.UnitPrice * this.Quantity * 0.10m;
        }

        internal void Cancel()
        {
            if (this.IsCancelled)
                throw new DomainException("Cannot cancel an already cancelled sale item.");

            this.IsCancelled = true;
        }
    }
}
