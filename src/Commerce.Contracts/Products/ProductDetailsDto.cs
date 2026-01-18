namespace Commerce.Contracts.Products;
public sealed record ProductDetailsDto(
    Guid ProductId,
    string Name,
    IEnumerable<string> Images,
    string Description
);