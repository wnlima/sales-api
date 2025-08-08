using Microsoft.AspNetCore.Builder;

namespace Ambev.ProductsDeveloperEvaluation.IoC
{
    public interface IModuleInitializer
    {
        void Initialize(WebApplicationBuilder builder);
    }
}
