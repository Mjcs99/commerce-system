namespace Commerce.Application.Products.Queries;

public sealed record GetProductsQuery(
    string? SearchTerm,
    List<string>? CategorySlugs,
    int Page,
    int PageSize 
);
