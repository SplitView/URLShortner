namespace RedirectLog.Domain.Entities
{
    public class CustomUrl : BaseEntity
    {
        public CustomUrl()
        {
            Redirections = new HashSet<Redirection>();
        }

        public string OriginalURL { get; set; }
        public string UniqueKey { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ICollection<Redirection> Redirections { get; set; }
    }
}
