using System.Reflection;

using Ambev.UsersDeveloperEvaluation.IoC.ModuleInitializers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.UsersDeveloperEvaluation.IoC
{
    public static class DependencyResolver
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder, params Assembly[] assemblies)
        {
            new ApplicationModuleInitializer().Initialize(builder);
            new InfrastructureModuleInitializer().Initialize(builder);
            new WebApiModuleInitializer().Initialize(builder);
            new DomainModuleInitializer().Initialize(builder);
            new EventModuleInitializer().Initialize(builder);

            builder.Services.AddAutoMapper(x => x.AddMaps(assemblies));
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies);
            });
        }
    }
}