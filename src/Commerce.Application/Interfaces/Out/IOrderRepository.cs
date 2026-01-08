using Commerce.Domain.Entities;

namespace Commerce.Application.Interfaces.Out;
public interface IOrderRepository
{
    public void AddOrder(Order order);
}