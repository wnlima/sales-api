using Ambev.DeveloperEvaluation.Domain.Common.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Common.Events
{
    public class DomainEventUtils
    {
        public static async Task PublishAllDomainEventsAsync<T>(IDomainEventPublisher publisher, T entity, CancellationToken cancellationToken) where T : BaseEntity
        {
            await Parallel.ForEachAsync(entity.DomainEvents, cancellationToken, async (domainEvent, ct) =>
            {
                await publisher.PublishAsync(domainEvent, ct);
            });

            entity.ClearDomainEvents();
        }
    }
}