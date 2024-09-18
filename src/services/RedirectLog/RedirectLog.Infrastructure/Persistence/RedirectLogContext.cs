using Microsoft.EntityFrameworkCore;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Domain.Entities;
using System.Reflection;

namespace RedirectLog.Infrastructure.Persistence;

public class RedirectLogContext(DbContextOptions<RedirectLogContext> options) : DbContext(options), IRedirectLogContext
{
    public DbSet<CustomUrl> CustomUrls { get; set; }
    public DbSet<Redirection> Redirections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
}