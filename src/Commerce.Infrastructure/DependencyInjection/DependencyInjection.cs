namespace Commerce.Infrastructure;
using Commerce.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;     
using Commerce.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Commerce.Infrastructure.Images;
using Commerce.Application.Images;
using Commerce.Infrastructure.Options;
using Commerce.Application.Interfaces.Out;
using Commerce.Infrastructure.Storage;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CommerceDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CommerceDb")));
        services.AddScoped<IProductRepository, EfProductRepository>();
        services.Configure<BlobStorageOptions>(options =>
        {
            configuration.GetSection("BlobStorage").Bind(options);
        });
        services.AddScoped<IProductImageStorage, AzureBlobStorage>();
        services.AddSingleton<IProductImageUriBuilder, AzureBlobProductImageUriBuilder>();

        return services;
    }
}