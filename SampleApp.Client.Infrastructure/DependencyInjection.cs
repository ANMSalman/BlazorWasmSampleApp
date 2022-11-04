using Microsoft.Extensions.DependencyInjection;
using SampleApp.Client.Application.Managers;
using SampleApp.Client.Infrastructure.Managers;

namespace SampleApp.Client.Infrastructure;
public static class DependencyInjection
{
    public static void AddClientInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductsManager, ProductsManager>();
    }
}
