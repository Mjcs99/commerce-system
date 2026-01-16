using Commerce.Application.Orders.Commands;
<<<<<<< HEAD
=======
using Commerce.Application.Orders.Results;
>>>>>>> 2c1c6145c851b9f6ff3f8050d464ae51db9a7269

namespace Commerce.Application.Interfaces.In;

public interface IOrderService
{
<<<<<<< HEAD
    Task<Guid> CreateOrderAsync(CreateOrderCommand command, CancellationToken ct);
=======
    Task<CreateOrderResult> CreateOrderAsync(CreateOrderCommand command, CancellationToken ct);
>>>>>>> 2c1c6145c851b9f6ff3f8050d464ae51db9a7269
}