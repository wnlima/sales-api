using Ambev.DeveloperEvaluation.WebApi.Common.HealthChecks;
using Ambev.DeveloperEvaluation.WebApi.Common.Logging;
using Ambev.ProductsDeveloperEvaluation.Application;
using Ambev.ProductsDeveloperEvaluation.Infrastructure;
using Ambev.ProductsDeveloperEvaluation.IoC;
using Ambev.ProductsDeveloperEvaluation.WebApi.Middleware;

using Microsoft.EntityFrameworkCore;

using Serilog;

namespace Ambev.ProductsDeveloperEvaluation.WebApi
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
