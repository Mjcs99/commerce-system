namespace Commerce.Application.Products.Results;
public sealed record AddImageResult(
    bool Success,
    Guid? ImageId,
    string? UrlOrError
);
