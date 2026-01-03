namespace Commerce.Contracts.Products;
public sealed record CreateProductDto(
    string Name,
    string Sku,
    decimal Price
);