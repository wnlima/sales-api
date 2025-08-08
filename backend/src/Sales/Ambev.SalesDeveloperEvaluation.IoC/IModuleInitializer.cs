using Microsoft.AspNetCore.Builder;

namespace Ambev.SalesDeveloperEvaluation.IoC
{
    public interface IModuleInitializer
    {
        void Initialize(WebApplicationBuilder builder);
    }
}
