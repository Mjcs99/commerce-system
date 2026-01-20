public sealed record PlaceOrderRequest(
    IReadOnlyList<OrderLineRequest> Items);

public sealed record OrderLineRequest(
    Guid ProductId,
    int Quantity);