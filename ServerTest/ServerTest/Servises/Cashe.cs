using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using ServerTest.Models;
using AppContext = System.AppContext;

namespace ServerTest.Servises;

public class Cashe
{
    private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private static ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public static async Task<List<MyCurrency>> GetOrCreate(KeyObj key)
    {
        List<MyCurrency> cacheEntry;

        if (!_cache.TryGetValue(key, out cacheEntry))// Ищем ключ в кэше.
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Ключ отсутствует в кэше, поэтому получаем данные.
                    AddOrGetInDB inDb = new AddOrGetInDB();
                    cacheEntry = inDb.GetCurrencies(key);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Храним в кэше в течении этого времени, сбрасываем время при обращении.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(360))
                        // Удаляем из кэша по истечении этого времени, независимо от скользящего таймера.
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(400));
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