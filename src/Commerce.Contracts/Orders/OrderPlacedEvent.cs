namespace Commerce.Contracts.Orders;
public sealed record OrderPlacedEvent(
    Guid OrderId,
    Guid CustomerId,
    IReadOnlyList<OrderPlacedItem> Items
);