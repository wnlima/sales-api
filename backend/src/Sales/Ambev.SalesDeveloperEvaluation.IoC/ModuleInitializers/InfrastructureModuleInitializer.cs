using Ambev.SalesDeveloperEvaluation.Domain.Repositories;
using Ambev.SalesDeveloperEvaluation.Infrastructure;
using Ambev.SalesDeveloperEvaluation.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ambev.SalesDeveloperEvaluation.IoC.ModuleInitializers
{
    public class InfrastructureModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsEnvironment("Test"))
                builder.Services.AddDbContext<DefaultContext>(options =>
                        options.UseNpgsql(
                            builder.Configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly("Ambev.SalesDeveloperEvaluation.Infrastructure")
                        )
                    );

            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
            builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        }
    }
}