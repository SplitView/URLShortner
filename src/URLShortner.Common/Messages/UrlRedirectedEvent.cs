namespace URLShortner.Common.Messages;

public class UrlRedirectedEvent(string customUrlId)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;
    public string CustomUrlId { get; set; } = customUrlId;
}