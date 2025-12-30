namespace Commerce.Application.Interfaces;

public interface IProductImageStorage
{
    Task<string> UploadAsync(
        Guid productId,
        Stream content,
        string contentType,
        string fileExtension,
        bool isPrimary,
        CancellationToken ct);
        string GetUrl(string blobName);
}