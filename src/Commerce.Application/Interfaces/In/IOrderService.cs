using Commerce.Application.Orders.Commands;
using Commerce.Application.Orders.Results;

namespace Commerce.Application.Interfaces.In;

public interface IOrderService
{
    Task<CreateOrderResult> CreateOrderAsync(CreateOrderCommand command);
}