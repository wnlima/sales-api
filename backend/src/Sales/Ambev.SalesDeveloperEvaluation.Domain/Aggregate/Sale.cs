using Ambev.DeveloperEvaluation.Domain.Common.Entities;
using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;
using Ambev.SalesDeveloperEvaluation.Domain.Entities;
using Ambev.SalesDeveloperEvaluation.Domain.Events;

namespace Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate
{
    public class Sale : BaseCustomerIdentity, IAggregateRoot, IIdentifier
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public int TotalItems { get; private set; }
        public decimal TotalDiscounts { get; private set; }
        public Guid BranchId { get; private set; }
        public bool IsCancelled { get; private set; }
        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
        private Sale() { }

        public static string GenerateSaleNumber()
        {
            return $"{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        }

        public static Sale Create(Guid customerId, Guid branchId, IEnumerable<SaleItem> items)
        {
            if (items == null || !items.Any())
                throw new DomainException("Sale must have at least one item.");

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = GenerateSaleNumber(),
                CustomerId = customerId,
                SaleDate = DateTime.UtcNow,
                BranchId = branchId,
            };

            foreach (var i in items)
                sale.AddItem(sale.Id, i.ProductId, i.Quantity, i.UnitPrice);

            sale.ComputeTotals();
            sale.AddDomainEvent(new SaleCreatedEvent(sale));

            return sale;
        }

        public void AddItem(Guid saleId, Guid productId, int quantity, decimal unitPrice)
        {
            if (_items.Any(i => i.ProductId == productId))
                throw new DomainException("Item with the same product already exists in the sale.");

            var item = SaleItem.Create(saleId, productId, quantity, unitPrice);

            _items.Add(item);
            AddDomainEvent(new SaleItemAddedEvent(item));
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new DomainException("Cannot cancel an already cancelled sale.");

            IsCancelled = true;

            foreach (var i in _items)
                i.Cancel();

            AddDomainEvent(new SaleCancelledEvent(this));

            foreach (var i in _items)
                AddDomainEvent(new SaleItemCancelledEvent(i));
        }

        private void ComputeTotals()
        {
            TotalAmount = Items.Sum(item => item.TotalAmount);
            TotalDiscounts = Items.Sum(item => item.Discount);
            TotalItems = Items.Sum(item => item.Quantity);
        }
    }
}