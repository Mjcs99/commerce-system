using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedProductsAsync(CommerceDbContext db, int count = 3)
    {   

        // Seed only if empty
        if (await db.Products.AnyAsync())
            return;

        var productTypes = new[] { "T-Shirt", "Mug", "Sticker", "Hoodie", "Cap", "Poster" };

        var products = new List<Product>(productTypes.Length * count);
        var inventory = new List<InventoryItem>(productTypes.Length * count);
        var skuCounter = 1;
        foreach (var type in productTypes)
        {
            var category = Category.Create(type, type.ToLower());
            db.Category.Add(category);
            for (int i = 1; i <= count; i++)
            {
                var name = $"{type} {i}";
                var sku = $"SKU-{skuCounter:D4}";    
                var price = Math.Round(5m + (i * 0.75m), 2);
                var product = Product.Create(sku, name, category.Id, price, "Clothing product");
                products.Add(product);
                inventory.Add(new InventoryItem{
                    ProductId = product.Id,
                    QuantityAvailable = 10
                });
                skuCounter++;
            }
        }
        db.InventoryItems.AddRange(inventory);
        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }

}
