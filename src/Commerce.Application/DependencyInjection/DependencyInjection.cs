using Commerce.Application.Interfaces;
using Commerce.Application.Interfaces.In;
using Commerce.Application.Interfaces.Out;
using Microsoft.Extensions.DependencyInjection;
namespace Commerce.Application.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        
        return services;
    }
}