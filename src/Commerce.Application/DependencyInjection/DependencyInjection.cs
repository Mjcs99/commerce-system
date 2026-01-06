using Commerce.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Commerce.Application.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}