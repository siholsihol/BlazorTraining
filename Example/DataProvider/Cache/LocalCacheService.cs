using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataProvider.Cache
{
    public class LocalCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public LocalCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key) =>
            _cache.Get<T>(key);

        public Task<T?> GetAsync<T>(string key, CancellationToken token = default) =>
            Task.FromResult(Get<T>(key));

        public void Refresh(string key) =>
            _cache.TryGetValue(key, out _);

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            Refresh(key);
            return Task.CompletedTask;
        }

        public void Remove(string key) =>
            _cache.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
        {
            if (slidingExpiration is null)
            {
                // TODO: add to appsettings?
                slidingExpiration = TimeSpan.FromMinutes(10); // Default expiration time of 10 minutes.
            }

            _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
        {
            Set(key, value, slidingExpiration);
            return Task.CompletedTask;
        }
    }
}