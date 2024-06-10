namespace IoTControlTower.Domain.Interface.CachingRepository;

public interface ICachingRepository
{
    Task SetAsync(string key, string value);
    Task<string> GetAsync(string key);
    Task RemoveAsync(string key);
    Task UpdateAsync(string key, string value);
}
