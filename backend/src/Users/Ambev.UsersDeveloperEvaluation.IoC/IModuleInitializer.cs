using Microsoft.AspNetCore.Builder;

namespace Ambev.UsersDeveloperEvaluation.IoC
{
    public interface IModuleInitializer
    {
        void Initialize(WebApplicationBuilder builder);
    }
}
