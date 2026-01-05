using System.ComponentModel;

namespace Commerce.Contracts.Products;

public record ProductDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal PriceAmount,
    string? PrimaryImageUrl = null
);
