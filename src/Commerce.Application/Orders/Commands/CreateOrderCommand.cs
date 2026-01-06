namespace Commerce.Application.Orders.Commands;
public sealed record CreateOrderCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
);