namespace Commerce.Contracts.Products;
public sealed record ProductDetailsDto(
    Guid ProductId,
    string Name,
    decimal Price,
    IEnumerable<string> Images,
    string Description
);