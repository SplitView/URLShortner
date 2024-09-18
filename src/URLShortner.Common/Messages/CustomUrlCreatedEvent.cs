namespace URLShortner.Common.Messages;

public class CustomUrlCreatedEvent(string customUrlId, string originalURL, string uniqueKey, DateTime expiryDate)
{
    public string CustomUrlId { get; set; } = customUrlId;
    public string OriginalURL { get; set; } = originalURL;
    public string UniqueKey { get; set; } = uniqueKey;
    public DateTime ExpiryDate { get; set; } = expiryDate;
}