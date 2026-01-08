using Microsoft.EntityFrameworkCore;
using Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Commerce.Infrastructure.Persistence.Configurations;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.ProductId).IsRequired();
        builder.Property(pi => pi.BlobName).IsRequired().HasMaxLength(1024);
        builder.Property(pi => pi.IsPrimary).IsRequired();
        builder.HasIndex(pi => new { pi.ProductId, pi.IsPrimary });   
    }
}