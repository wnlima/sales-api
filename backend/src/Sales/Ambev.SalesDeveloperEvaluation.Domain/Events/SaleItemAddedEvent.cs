using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;
using Ambev.SalesDeveloperEvaluation.Domain.Entities;

namespace Ambev.SalesDeveloperEvaluation.Domain.Events
{
    public record SaleItemAddedEvent(SaleItem Item) : IDomainEvent;
}