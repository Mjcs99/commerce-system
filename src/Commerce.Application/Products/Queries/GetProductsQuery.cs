namespace Commerce.Application.Products.Queries;

public sealed record GetProductsQuery(
    string? SearchTerm,
    string? CategorySlug,
    int Page,
    int PageSize 
);
