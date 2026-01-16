namespace Commerce.Application.Products.Results;
public sealed record AddProductResult(
    bool Success,
    Guid ProductId,
    string? ErrorMessage = null
);