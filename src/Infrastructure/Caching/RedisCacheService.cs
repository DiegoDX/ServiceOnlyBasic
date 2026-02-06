using System;
using System.Collections.Generic;
using System.Text;
using Caching.Extensions;
using Core.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Caching
{
    public class RedisCacheService: ICacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            //var json = await _cache.GetStringAsync(key);
            //return json is null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(json);
            return await _cache.GetAsync<T>(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            //var options = new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = ttl
            //};
            //var json = System.Text.Json.JsonSerializer.Serialize(value);
            //await _cache.SetStringAsync(key, json, options);
            await _cache.SetAsync<T>(key, value, ttl);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
