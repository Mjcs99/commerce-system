namespace Commerce.Application.Products.Queries;

public sealed record GetProductsQuery(
    string? SearchTerm,
    int Page,
    int PageSize 
);
