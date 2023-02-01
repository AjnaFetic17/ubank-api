using Microsoft.Extensions.Caching.Memory;
using ubank_api.Services.Interfaces;

namespace ubank_api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void AddToCache(string key, object value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                    .SetPriority(CacheItemPriority.Normal);

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public object GetFromCache(string key)
        {
            return _memoryCache.Get(key)!;
        }

        public void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
