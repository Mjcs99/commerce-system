namespace Commerce.Domain.Entities;

public sealed class ProductImage
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string BlobName { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    private ProductImage() { } 

    internal ProductImage(Guid productId, string blobName, Guid imageId, bool isPrimary)
    {
        Id = imageId;
        ProductId = productId;
        BlobName = blobName;
        IsPrimary = isPrimary;
    }

    public void SetPrimary() => IsPrimary = true;
    public void UnsetPrimary() => IsPrimary = false;
}
