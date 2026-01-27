using Commerce.Domain.Entities;
using Commerce.Infrastructure.Options;
using Commerce.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OptionsExtensions = Microsoft.Extensions.Options.Options;

namespace Commerce.Infrastructure.Persistence;

public class SeedData
{
    private readonly IOptions<BlobStorageOptions> _options;
    private readonly CommerceDbContext _db;
    public SeedData(CommerceDbContext db, IOptions<BlobStorageOptions> options)
    {
        _db = db;
        _options = options;
    }
    public async Task SeedProductsAsync(int count = 3)
    {   

        // Seed only if empty
        if (await _db.Products.AnyAsync())
            return;
        var imageStorage = new AzureBlobStorage(_options);

        var typesAndPhotos = new[] {("T-Shirt", "../Commerce.Infrastructure/Persistence/TestImages/tshirt.png"), ("Cap", "../Commerce.Infrastructure/Persistence/TestImages/cap.png"), ("Blazer", "../Commerce.Infrastructure/Persistence/TestImages/blazer.png"), ("Jacket", "../Commerce.Infrastructure/Persistence/TestImages/jacket.png") };
        var products = new List<Product>(typesAndPhotos.Length * count);
        var inventory = new List<InventoryItem>(typesAndPhotos.Length * count);
        var skuCounter = 1;
        foreach (var (type, imagePath) in typesAndPhotos)
        {
            
            
            var category = Category.Create(type, type.ToLower());
            _db.Category.Add(category);
            for (int i = 1; i <= count; i++)
            {
                
                var name = $"{type} {i}";
                var sku = $"SKU-{skuCounter:D4}";    
                var price = Math.Round(5m + (i * 0.75m), 2);
                var product = Product.Create(sku, name, category.Id, price, "Clothing product");
                products.Add(product);
                var imageId = new Guid();
                var blobName = $"products/{product.Id}/images/{imageId}.png";
                await using var stream = File.OpenRead(imagePath);
                await imageStorage.UploadAsync(blobName, stream, "image/png");
                product.AddImage(blobName, imageId, true);
                inventory.Add(new InventoryItem{
                    ProductId = product.Id,
                    QuantityAvailable = 10
                });
                skuCounter++;
            }
        }
        _db.InventoryItems.AddRange(inventory);
        _db.Products.AddRange(products);
        await _db.SaveChangesAsync();
    }

}
