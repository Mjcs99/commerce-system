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
using Commerce.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Commerce.Infrastructure.Messaging;
using Azure.Messaging.ServiceBus;
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
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<ICustomerRepository, EfCustomerRepository>();
        services.AddScoped<IOutbox, EfOutbox>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();    
        // Fix 
        services.AddSingleton(_ => new ServiceBusClient(configuration["ServiceBus:ConnectionString"]));
        services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
        services.Configure<ServiceBusOptions>(
            "OrderPlaced",
            configuration.GetSection("AzureServiceBus:OrderPlaced"));

        services.Configure<ServiceBusOptions>(
            "Email",
            configuration.GetSection("AzureServiceBus:Email"));
 
        return services;
    }
}