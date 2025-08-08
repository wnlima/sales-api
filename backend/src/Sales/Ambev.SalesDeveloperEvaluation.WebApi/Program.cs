using Ambev.DeveloperEvaluation.WebApi.Common.HealthChecks;
using Ambev.DeveloperEvaluation.WebApi.Common.Logging;
using Ambev.SalesDeveloperEvaluation.Application;
using Ambev.SalesDeveloperEvaluation.Infrastructure;
using Ambev.SalesDeveloperEvaluation.IoC;
using Ambev.SalesDeveloperEvaluation.WebApi.Middleware;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;

namespace Ambev.SalesDeveloperEvaluation.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting web application");

                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
                builder.AddDefaultLogging();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                builder.AddBasicHealthChecks();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Insira o token recuperado na Users API com 'Bearer ' no campo de texto abaixo.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });


                builder.RegisterDependencies(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

                var app = builder.Build();

                var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS") ?? "false";
                if (bool.Parse(runMigrations))
                {
                    Log.Information("Running migrations");
                    using var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                    db.Database.Migrate();
                }

                app.UseMiddleware<ErrorExceptionMiddleware>();
                app.UseMiddleware<ValidationExceptionMiddleware>();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseBasicHealthChecks();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
