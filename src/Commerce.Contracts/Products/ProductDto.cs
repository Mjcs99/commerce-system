namespace Commerce.Contracts.Products;

public record ProductDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal PriceAmount,
    string? ImagesUrl = null
);
