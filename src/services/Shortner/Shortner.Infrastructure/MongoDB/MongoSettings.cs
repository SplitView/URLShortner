namespace Shortner.Infrastructure.MongoDB;

public class MongoSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CustomURLCollectionName { get; set; }
    public string RedirectionLogCollectionName { get; set; }
}