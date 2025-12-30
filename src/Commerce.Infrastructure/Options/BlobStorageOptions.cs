namespace Commerce.Infrastructure.Options;

public sealed class BlobStorageOptions
{
    public string AccountName { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}