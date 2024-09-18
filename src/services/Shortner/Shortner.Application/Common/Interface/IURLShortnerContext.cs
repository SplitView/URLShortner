using MongoDB.Driver;

namespace Shortner.Application.Interface;

public interface IURLShortnerContext
{
    IMongoCollection<Domain.Entities.CustomURL> CustomURLs { get; set; }
}