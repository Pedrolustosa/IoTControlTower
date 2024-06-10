using Microsoft.Extensions.Caching.Distributed;
using IoTControlTower.Domain.Interface.CachingRepository;

namespace IoTControlTower.Infra.Data.Repositories.CachingRepository;

public class CachingRepository(IDistributedCache distributedCache) : ICachingRepository
{
    private readonly IDistributedCache _cache = distributedCache;

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
        SlidingExpiration = TimeSpan.FromSeconds(1200)
    };

    public async Task<string> GetAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task UpdateAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }
}
