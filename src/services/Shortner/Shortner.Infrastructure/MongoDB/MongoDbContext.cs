using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shortner.Application.Interface;
using Shortner.Domain.Entities;

namespace Shortner.Infrastructure.MongoDB
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
        }

        public IMongoCollection<CustomURL> CustomURLs { get; set; }

        private IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}
