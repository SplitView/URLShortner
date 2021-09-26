using MongoDB.Driver;

namespace URLShortner.Application.Common.Interface
{
    public interface IURLShortnerContext
    {
        IMongoCollection<Domain.Entities.CustomURL> CustomURLs { get; set; }
        IMongoCollection<Domain.Entities.Redirection> RedirectionLog { get; set; }
    }
}
