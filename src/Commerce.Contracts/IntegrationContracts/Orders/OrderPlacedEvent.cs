namespace Commerce.Contracts.IntegrationContracts.Orders;
public sealed record OrderPlacedEvent(
    Guid OrderId,
    Guid CustomerId,
    IReadOnlyList<OrderPlacedItem> Items
);
public sealed record OrderPlacedItem(
    Guid ProductId,
    int Qauntity,
    decimal UnitPrice
);