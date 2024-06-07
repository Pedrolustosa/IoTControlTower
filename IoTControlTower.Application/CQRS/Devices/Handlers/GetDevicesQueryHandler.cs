using MediatR;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;
using IoTControlTower.Domain.Interface.CachingRepository;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDevicesQueryHandler(IDeviceDapperRepository devicesDapperRepository, 
                                    ILogger<GetDevicesQueryHandler> logger,
                                    ICachingRepository cachingRepository) : IRequestHandler<GetDevicesQuery, IEnumerable<Device>>
{
    private readonly ILogger<GetDevicesQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ICachingRepository _cachingRepository = cachingRepository ?? throw new ArgumentNullException(nameof(cachingRepository));
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository ?? throw new ArgumentNullException(nameof(devicesDapperRepository));

    public async Task<IEnumerable<Device>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesQuery");

        try
        {
            IEnumerable<Device>? devices = null;
            string cacheKey = "Devices";
            var cachedDevices = await _cachingRepository.GetAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedDevices))
            {
                try
                {
                    var devicesFromCache = JsonSerializer.Deserialize<IEnumerable<Device>>(cachedDevices);
                    if (devicesFromCache is not null)
                    {
                        _logger.LogInformation("Retrieved {Count} devices from cache", devicesFromCache.Count());
                        return devicesFromCache;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize devices from cache");
                }
            }

            devices = await _devicesDapperRepository.GetDevicesAsync();
            if (devices is null || !devices.Any())
            {
                _logger.LogWarning("No devices found");
                return new List<Device>();
            }

            var serializedDevices = JsonSerializer.Serialize(devices);
            await _cachingRepository.SetAsync(cacheKey, serializedDevices);

            _logger.LogInformation("GetDevicesQuery handled successfully, found {Count} devices", devices.Count());
            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDevicesQuery");
            throw;
        }
    }
}
