using System;

namespace URLShortner.Domain.Entities
{
    public class Redirection : BaseEntity
    {
        public string CustomUrlId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
