using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public class CommerceDbContext : DbContext
{
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(b =>
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.Sku).IsRequired().HasMaxLength(64);
            b.HasIndex(p => p.Sku).IsUnique();

            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.PriceAmount).IsRequired();

            b.Navigation(p => p.Images)
             .UsePropertyAccessMode(PropertyAccessMode.Field);

            b.HasMany(p => p.Images)
             .WithOne()
             .HasForeignKey(pi => pi.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductImage>(b =>
        {
            b.HasKey(pi => pi.Id);

            b.Property(pi => pi.ProductId).IsRequired();
            b.Property(pi => pi.BlobName).IsRequired().HasMaxLength(1024);
            b.Property(pi => pi.IsPrimary).IsRequired();

            b.HasIndex(pi => new { pi.ProductId, pi.IsPrimary });
        });
    }
}

