namespace Commerce.Application.Interfaces.Out;

public interface IBlobStorage
{
    Task<BlobUploadResult> UploadAsync(
        string blobName,
        Stream content,
        string contentType,
        CancellationToken ct = default);

    Task DeleteIfExistsAsync(string blobName, CancellationToken ct = default);
}

public sealed record BlobUploadResult(string Url);
