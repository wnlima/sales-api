using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;
using Ambev.ProductsDeveloperEvaluation.Domain.Entities;

namespace Ambev.ProductsDeveloperEvaluation.Domain.Events
{
    public record ProductLowStockEvent(Product Item) : IDomainEvent;
}