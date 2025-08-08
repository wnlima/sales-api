using Ambev.DeveloperEvaluation.Domain.Common.Security;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.UsersDeveloperEvaluation.IoC.ModuleInitializers
{
    public class ApplicationModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        }
    }
}