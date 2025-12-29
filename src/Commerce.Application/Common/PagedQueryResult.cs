namespace Commerce.Application.Common;
public sealed record PagedQueryResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount
);