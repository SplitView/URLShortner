namespace Shortner.Domain.Entities;

public class CustomURL : BaseEntity
{
    public string OriginalURL { get; set; }
    public string UniqueKey { get; set; }
    public DateTime ExpiryDate { get; set; }
}