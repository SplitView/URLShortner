namespace URLShortner.Common.Messages;

public class UrlRedirectedEvent
{
    public Guid Id { get; private set; }
    public DateTime TimeStamp { get; private set; }
    public string CustomUrlId { get; set; }
    public UrlRedirectedEvent(string customUrlId)
    {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            CustomUrlId = customUrlId;
        }
}