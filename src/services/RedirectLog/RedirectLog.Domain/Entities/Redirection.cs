namespace RedirectLog.Domain.Entities;

public class Redirection : BaseEntity
{
    public string CustomUrlId { get; set; }
    public DateTime TimeStamp { get; set; }
    public CustomUrl CustomUrl { get; set; }
}