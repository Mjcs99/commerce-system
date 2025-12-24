namespace Commerce.Application.Products.Results;

public sealed record ProductResult(
    Guid Id,
    string Name,
    string Sku,
    decimal Price
);
