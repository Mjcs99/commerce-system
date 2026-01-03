using System.IO.Pipelines;

namespace Commerce.Domain.Entities;

public sealed class Product
{
    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.ToList();

    public Guid Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal PriceAmount { get; private set; }

    private Product() { } 

    public Product(Guid id, string sku, string name, decimal priceAmount)
    {
        Id = id;
        Sku = sku;
        Name = name;
        PriceAmount = priceAmount;
    }
    public static Product Create(string sku, string name, decimal priceAmount)
        => new(Guid.NewGuid(), sku, name, priceAmount);
    public ProductImage? GetPrimaryImage()
    {
        return _images.FirstOrDefault(i => i.IsPrimary);
    }
    public ProductImage AddImage(string blobName, Guid imageId, bool makePrimary)
    {
        if (makePrimary) SetAllImagesNonPrimary();
        
        var image = new ProductImage(productId: Id, blobName: blobName, imageId: imageId, isPrimary: makePrimary);
        
        _images.Add(image);
        
        return image;
    }

    public void SetPrimaryImage(Guid imageId)
    {
        var target = _images.FirstOrDefault(i => i.Id == imageId)
            ?? throw new InvalidOperationException("Image not found for this product.");

        SetAllImagesNonPrimary();
        target.SetPrimary();
    }

    private void SetAllImagesNonPrimary()
    {
        foreach (var img in _images)
            img.UnsetPrimary();
    }
}
