using Commerce.Domain.Entities;
using Commerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedProductsAsync(CommerceDbContext db, int count = 3)
    {

        //db.Products.RemoveRange(db.Products);
        //await db.SaveChangesAsync();
        // Safety: don't reseed if products already exist
        if (await db.Products.AnyAsync())
            return;

        var products = new List<Product>(count);
        var product_types = new[] { "T-Shirt", "Mug", "Sticker", "Hoodie", "Cap", "Poster" };
        var rnd = new Random();
        foreach (var product_type in product_types)
        {
            for (int i = 1; i <= count; i++)
            {
                Guid id = Guid.NewGuid();
                var name = $"{product_type} {i}";
                var sku = $"SKU-{i:D4}";
                var price = Math.Round(5m + (i * 0.75m), 2);

                products.Add(new Product(id, name, sku, price));
            }
        }
        db.Products.AddRange(products);
        await db.SaveChangesAsync();
    }
}
