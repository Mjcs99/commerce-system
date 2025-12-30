namespace Commerce.Infrastructure.Images;
using Commerce.Infrastructure.Options;
using Commerce.Application.Images;
using Microsoft.Extensions.Options;
public sealed class AzureBlobProductImageUriBuilder : IProductImageUriBuilder
{
    private readonly BlobStorageOptions _options;

    public AzureBlobProductImageUriBuilder(IOptions<BlobStorageOptions> options)
    {
        _options = options.Value;
    }

    public string BuildUri(string? primaryImageBlobName)
    {
        if (string.IsNullOrEmpty(primaryImageBlobName))
        {
            return string.Empty;
        }

        return $"https://{_options.AccountName}.blob.core.windows.net/{_options.ContainerName}/{primaryImageBlobName}";
    }
}