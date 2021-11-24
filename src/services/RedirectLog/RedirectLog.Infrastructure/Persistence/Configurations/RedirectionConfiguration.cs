using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Infrastructure.Persistence.Configurations
{
    public class RedirectionConfiguration : IEntityTypeConfiguration<Redirection>
    {
        public void Configure(EntityTypeBuilder<Redirection> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.CustomUrl)
                .WithMany(x => x.Redirections)
                .HasForeignKey(x => x.CustomUrlId);

            builder.Property(x => x.TimeStamp).IsRequired();
        }
    }
}
