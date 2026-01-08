using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Infrastructure.Persistence;

public class CommerceDbContext : DbContext
{
    public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<InventoryItem> InventoryItem => Set<InventoryItem>();

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
            b.Property(p => p.CategoryId).IsRequired();
            b.HasOne(p => p.Category)
                .WithMany()                
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
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

        modelBuilder.Entity<Category>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Name).IsRequired().HasMaxLength(100);
            b.Property(c => c.Slug).IsRequired().HasMaxLength(100);
            b.HasIndex(c => c.Slug).IsUnique();
        });

        modelBuilder.Entity<Customer>(b => 
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.ExternalUserId).IsRequired().HasMaxLength(100);
            b.HasIndex(c => c.ExternalUserId).IsUnique();
            b.Property(c => c.FirstName).HasMaxLength(50);
            b.Property(c => c.LastName).HasMaxLength(50);
            b.Property(c => c.Email).IsRequired().HasMaxLength(256);
            b.HasIndex(c => c.Email).IsUnique();
            b.Property(c => c.CreatedAtUtc).IsRequired();
        });

        modelBuilder.Entity<InventoryItem>(b =>
        {
            b.HasKey(ii => ii.ProductId);
            b.Property(ii => ii.QuantityAvailable).IsRequired();
            b.HasOne<Product>()
                .WithOne()
                .HasForeignKey<InventoryItem>(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.CustomerId).IsRequired();
            b.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();
            b.Property(o => o.CreatedAtUtc).IsRequired();
            b.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            b.HasMany(o => o.Items)
                .WithOne()                        
                .HasForeignKey(oi => oi.OrderId)   
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            b.Navigation(o => o.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });


        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(oi => oi.Id);
            b.Property(oi => oi.OrderId).IsRequired();
            b.Property(oi => oi.ProductId).IsRequired();
            b.Property(oi => oi.Quantity).IsRequired();
            b.Property(oi => oi.UnitPriceAmount).IsRequired();
            b.HasOne<Order>()
                .WithMany(o => o.Items)              
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            b.HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
        });

    }
}

