using Ambev.DeveloperEvaluation.Domain.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Common.Validation;
using Ambev.SalesDeveloperEvaluation.Infrastructure.Events;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.SalesDeveloperEvaluation.IoC.ModuleInitializers
{
    public class EventModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddScoped<IDomainEventPublisher, ConsoleDomainEventPublisher>();
        }
    }
}