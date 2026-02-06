using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed; 

namespace Caching.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan ttl)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl
            };

            var json = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, json, options);
        }

        public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var json = await cache.GetStringAsync(key);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
