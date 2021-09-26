using Microsoft.Extensions.Options;

using MongoDB.Driver;

using URLShortner.Application.Common.Interface;
using URLShortner.Domain.Entities;

namespace URLShortner.Infrastructure.MongoDB
{
    public class MongoDbContext : IURLShortnerContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbContext(IOptions<MongoSettings> options)
        {
            _mongoClient = new MongoClient(options.Value.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(options.Value.DatabaseName);

            CustomURLs = GetMongoCollection<CustomURL>(options.Value.CustomURLCollectionName);
            RedirectionLog = GetMongoCollection<Redirection>(options.Value.RedirectionLogCollectionName);
        }

        public IMongoCollection<CustomURL> CustomURLs { get; set; }
        public IMongoCollection<Redirection> RedirectionLog { get; set; }

        private IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}
