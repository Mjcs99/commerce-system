namespace Commerce.Contracts.Products;
public sealed record CreateProductDto(
    string Name,
    string Sku,
    string CategorySlug,
    decimal Price,
    string description
);