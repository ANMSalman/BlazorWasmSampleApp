using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Server.Application.Common;
using SampleApp.Server.Application.Services;
using SampleApp.Server.Infrastructure.Common;
using SampleApp.Server.Infrastructure.Persistence;
using SampleApp.Server.Infrastructure.Services;

namespace SampleApp.Server.Infrastructure;
public static class DependencyInjection
{
    public static void AddSeverInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SampleAppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DbConnection"),
                b => b.MigrationsAssembly(typeof(SampleAppDbContext).Assembly.FullName)));

        services.AddScoped<IImageUploader, ImageUploader>();
        services.AddScoped<IProductService, ProductService>();

    }
}
