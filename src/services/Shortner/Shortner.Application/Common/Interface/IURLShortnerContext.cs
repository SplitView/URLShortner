using MongoDB.Driver;
using Shortner.Domain.Entities;

namespace Shortner.Application.Interface;

public interface IURLShortnerContext
{
    IMongoCollection<CustomURL> CustomURLs { get; set; }
}