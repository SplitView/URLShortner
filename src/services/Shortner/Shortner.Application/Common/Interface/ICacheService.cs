using Microsoft.Extensions.Caching.Distributed;

namespace Shortner.Application.Interface;

public interface ICacheService
{
    /// <summary>
    /// Clears the cache with the key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task ClearAsync(string key);

    /// <summary>
    /// Fetch the catche of the key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns>Stored cache or null if no cache found</returns>
    Task<T> GetAsync<T>(string key) where T : class;

    /// <summary>
    /// Sets the cache with given cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options) where T : class;
}