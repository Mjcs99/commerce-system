namespace Commerce.Application.Products.Commands;

public sealed record CreateProductCommand(
    string Name,
    string Sku,
    string CategorySlug,
    decimal Price
);
