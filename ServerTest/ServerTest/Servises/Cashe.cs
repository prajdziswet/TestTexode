using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace ServerTest.Servises;

public class Cashe<TItem>
{
    private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
    {
        TItem cacheEntry;

        if (!_cache.TryGetValue(key, out cacheEntry))// Ищем ключ в кэше.
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Ключ отсутствует в кэше, поэтому получаем данные.
                    cacheEntry = await createItem();
                    _cache.Set(key, cacheEntry);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        return cacheEntry;
    }

}