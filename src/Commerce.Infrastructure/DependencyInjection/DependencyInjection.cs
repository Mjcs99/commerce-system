namespace Commerce.Infrastructure;
using Commerce.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;     
using Commerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Commerce.Infrastructure.Images;
using Commerce.Application.Images;
using Commerce.Infrastructure.Options;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.Configure<BlobStorageOptions>(options =>
        {
            configuration.GetSection("BlobStorage").Bind(options);
        });
        
        services.AddSingleton<IProductImageUriBuilder, AzureBlobProductImageUriBuilder>();

        return services;
    }
}