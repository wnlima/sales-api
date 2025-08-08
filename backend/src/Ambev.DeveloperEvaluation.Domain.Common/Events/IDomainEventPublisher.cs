using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;

namespace Ambev.DeveloperEvaluation.Domain.Common.Events
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent;
    }
}