using System.Text.Json;
using System.Text.Json.Serialization;

using Ambev.DeveloperEvaluation.Domain.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Common.Interfaces;

namespace Ambev.ProductsDeveloperEvaluation.Infrastructure.Events
{
    public class ConsoleDomainEventPublisher : IDomainEventPublisher
    {
        public Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent
        {
            Console.WriteLine($"[DOMAIN EVENT] {typeof(TEvent).Name} -> {DateTime.Now}");
            Console.WriteLine(JsonSerializer.Serialize(domainEvent, new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }));

            return Task.CompletedTask;
        }
    }
}