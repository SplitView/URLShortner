namespace URLShortner.Common.Messages;

public class CustomUrlCreatedEvent
{
    public CustomUrlCreatedEvent(string customUrlId, string originalURL, string uniqueKey, DateTime expiryDate)
    {
            CustomUrlId = customUrlId;
            OriginalURL = originalURL;
            UniqueKey = uniqueKey;
            ExpiryDate = expiryDate;
        }

    public string CustomUrlId { get; set; }
    public string OriginalURL { get; set; }
    public string UniqueKey { get; set; }
    public DateTime ExpiryDate { get; set; }
}