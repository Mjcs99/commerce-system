namespace Commerce.Application.Products.Commands;
public sealed record AddProductImageCommand(
    Guid ProductId,
    Stream Content,
    string FileName,
    string ContentType, 
    bool IsPrimary = false
);
