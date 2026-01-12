using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Commerce.Application.Interfaces.Out;
using Commerce.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Azure.Identity;
namespace Commerce.Infrastructure.Storage;

public sealed class AzureBlobStorage : IProductImageStorage
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorage(IOptions<BlobStorageOptions> options)
    {
        var o = options.Value;

        var serviceClient = new BlobServiceClient(new Uri($"https://{o.AccountName}.blob.core.windows.net"), new DefaultAzureCredential());
        _container = serviceClient.GetBlobContainerClient(o.ContainerName);
    }

    public async Task<BlobUploadResult> UploadAsync(
        string blobName,
        Stream content,
        string contentType,
        CancellationToken ct = default)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        
        var blob = _container.GetBlobClient(blobName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        // Overwrite true by default
        await blob.UploadAsync(content, uploadOptions, ct);

        return new BlobUploadResult(blob.Uri.ToString());
    }

    public async Task DeleteIfExistsAsync(string blobName, CancellationToken ct = default)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: ct);
    }
}
