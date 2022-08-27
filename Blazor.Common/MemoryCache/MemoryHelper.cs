using Microsoft.Extensions.Caching.Memory;

namespace Blazor.Common.MemoryCache
{
    public class MemoryHelper
    {
        private static IMemoryCache _memoryCache = null;
        static MemoryHelper()
        {
            if (_memoryCache == null)
            {
                _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
            }
        }
        public static void SetMemory(string key, object value, int expireMins)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(expireMins));
        }
        public static object GetMemory(string key)
        {
            return _memoryCache.Get(key);
        }
    }
}
