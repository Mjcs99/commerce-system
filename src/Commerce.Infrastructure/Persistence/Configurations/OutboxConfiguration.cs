namespace Commerce.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Commerce.Infrastructure.Persistence.Outbox;

public sealed class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Type).IsRequired();
        builder.Property(c => c.Payload).IsRequired();
        builder.Property(c => c.OccurredAtUtc).IsRequired();
        builder.HasIndex(x => x.DispatchedAtUtc);
    }
}