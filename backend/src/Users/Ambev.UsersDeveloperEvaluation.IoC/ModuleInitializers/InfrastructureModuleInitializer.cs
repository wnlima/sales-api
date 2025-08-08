using Ambev.UsersDeveloperEvaluation.Domain.Repositories;
using Ambev.UsersDeveloperEvaluation.Infrastructure;
using Ambev.UsersDeveloperEvaluation.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ambev.UsersDeveloperEvaluation.IoC.ModuleInitializers
{
    public class InfrastructureModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsEnvironment("Test"))
                builder.Services.AddDbContext<DefaultContext>(options =>
                        options.UseNpgsql(
                            builder.Configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly("Ambev.UsersDeveloperEvaluation.Infrastructure")
                        )
                    );

            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}