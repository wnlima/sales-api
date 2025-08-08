using System.Security.Claims;

using Ambev.DeveloperEvaluation.WebApi.Common.Security;
using Ambev.UsersDeveloperEvaluation.Domain.Enums;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.SalesDeveloperEvaluation.IoC.ModuleInitializers
{
    public class WebApiModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));

                options.AddPolicy("AdminOrManager", policy =>
                   policy.RequireClaim(ClaimTypes.Role, new string[] { UserRole.Admin.ToString(), UserRole.Manager.ToString() })
               );

                options.AddPolicy("AnyRole", policy =>
                    policy.RequireClaim(ClaimTypes.Role, new string[] { UserRole.Admin.ToString(), UserRole.Manager.ToString(), UserRole.Customer.ToString() })
                );
            });

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
        }
    }
}
