namespace Commerce.Application.Products.Queries;

public sealed record GetProductsQuery(
    int Page,
    int PageSize
);
