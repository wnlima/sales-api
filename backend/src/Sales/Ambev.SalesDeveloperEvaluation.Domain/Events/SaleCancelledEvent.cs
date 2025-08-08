using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;
using Ambev.SalesDeveloperEvaluation.Domain.SaleAggregate;

namespace Ambev.SalesDeveloperEvaluation.Domain.Events
{
    public record SaleCancelledEvent(Sale Sale) : IDomainEvent;
}