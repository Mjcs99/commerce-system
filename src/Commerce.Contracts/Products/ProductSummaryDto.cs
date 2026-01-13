namespace Commerce.Contracts.Products;

public record ProductSummaryDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal PriceAmount,
    string? PrimaryImageUrl = null
);
