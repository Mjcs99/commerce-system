using Commerce.Application.Orders.Commands;

namespace Commerce.Application.Interfaces.In;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(PlaceOrderRequest request, Guid customerId, CancellationToken ct);
}