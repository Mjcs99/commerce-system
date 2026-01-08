using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces.Out;
public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
}