namespace Commerce.Application.DTOs;

public record ProductDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal PriceAmount,
    string Currency
);
