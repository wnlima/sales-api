using Ambev.SalesDeveloperEvaluation.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.SalesDeveloperEvaluation.Tests.Integration.Auxiliary
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly List<Action<IServiceCollection>> _diBehaviors = new();

        public readonly IConfigurationRoot Configuration;

        public CustomWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<DefaultContext>(options => options.UseInMemoryDatabase("TestDatabase"));
                _diBehaviors.ForEach(d => d(services));
            })
            .UseConfiguration(Configuration);

            builder.UseEnvironment("Test");
        }

        internal void SetDIBehaviors(Action<IServiceCollection>[] behaviors)
        {
            _diBehaviors.AddRange(behaviors);
        }
    }
}