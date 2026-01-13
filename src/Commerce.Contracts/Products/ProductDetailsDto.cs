namespace Commerce.Contracts.Products;
public sealed record ProductDetailsDto(
    Guid ProductId,
    string description
);