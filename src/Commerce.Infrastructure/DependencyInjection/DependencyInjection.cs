namespace Commerce.Infrastructure;
using Commerce.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;     
using Commerce.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, EfProductRepository>();

        return services;
    }
}