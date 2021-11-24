using Microsoft.EntityFrameworkCore;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Application.Common.Interface
{
    public interface IRedirectLogContext
    {
        DbSet<CustomUrl> CustomUrls { get; set; }
        DbSet<Redirection> Redirections { get; set; }
    }
}
