namespace Commerce.Application.Orders.Results;
public sealed record CreateOrderResult(
    bool Success,
    Guid OrderId,
    string? ErrorMessage = null
);