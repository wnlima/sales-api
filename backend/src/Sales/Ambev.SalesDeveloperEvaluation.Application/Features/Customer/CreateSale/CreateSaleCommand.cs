using MediatR;

namespace Ambev.SalesDeveloperEvaluation.Application.Features.Customer.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalDiscounts { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<CreateSaleItemCommand> Items { get; set; }
    }
}