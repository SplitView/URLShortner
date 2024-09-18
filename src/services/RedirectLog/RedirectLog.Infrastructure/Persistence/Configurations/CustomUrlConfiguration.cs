using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Infrastructure.Persistence.Configurations;

public class CustomUrlConfiguration : IEntityTypeConfiguration<CustomUrl>
{
    public void Configure(EntityTypeBuilder<CustomUrl> builder)
    {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UniqueKey).IsRequired().HasMaxLength(6);

            builder.Property(x => x.OriginalURL).IsRequired();
            builder.Property(x=>x.ExpiryDate).IsRequired();
            builder.HasMany(x => x.Redirections)
                .WithOne(x => x.CustomUrl)
                .HasForeignKey(x => x.CustomUrlId);
        }
}