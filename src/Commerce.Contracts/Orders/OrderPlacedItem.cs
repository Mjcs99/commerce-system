namespace Commerce.Contracts.Orders;

public sealed record OrderPlacedItem(
    Guid ProductId,
    int Qauntity,
    decimal UnitPrice
);