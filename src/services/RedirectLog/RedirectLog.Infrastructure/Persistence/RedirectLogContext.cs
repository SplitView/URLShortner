using Microsoft.EntityFrameworkCore;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Domain.Entities;
using System.Reflection;

namespace RedirectLog.Infrastructure.Persistence;

public class RedirectLogContext : DbContext, IRedirectLogContext
{
    public RedirectLogContext(DbContextOptions<RedirectLogContext> options) : base(options)
    {

        }

    public DbSet<CustomUrl> CustomUrls { get; set; }
    public DbSet<Redirection> Redirections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
}