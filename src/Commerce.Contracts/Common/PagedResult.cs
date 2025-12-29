namespace Commerce.Contracts.Common;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int TotalCount,
    int PageSize = 20
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
