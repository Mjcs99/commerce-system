namespace Commerce.Application.Images;

public interface IProductImageUriBuilder
{
    string BuildUri(string? primaryImageBlobName, int sasMinutes);
}