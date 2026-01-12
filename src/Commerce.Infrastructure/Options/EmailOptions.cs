namespace Commerce.Infrastructure.Options;
public sealed class EmailOptions
{
    public string FromAddress { get; init; } = default!;
    public string ConnectionString { get; init; } = default!;
}
