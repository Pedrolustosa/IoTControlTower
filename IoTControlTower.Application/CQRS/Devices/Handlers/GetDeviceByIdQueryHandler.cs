using MediatR;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;
using IoTControlTower.Domain.Interface.CachingRepository;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDeviceByIdQueryHandler(IDeviceDapperRepository devicesDapperRepository,
                                       ILogger<GetDeviceByIdQueryHandler> logger,
                                       ICachingRepository cachingRepository) : IRequestHandler<GetDeviceByIdQuery, Device>
{
    private readonly ILogger<GetDeviceByIdQueryHandler> _logger = logger;
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository;
    private readonly ICachingRepository _cachingRepository = cachingRepository ?? throw new ArgumentNullException(nameof(cachingRepository));

    public async Task<Device> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDeviceByIdQuery for device ID: {Id}", request.DeviceId);

        try
        {
            Device? getDeviceByIdQuery = null;
            string cacheKey = $"Device: {request.DeviceId}";

            _logger.LogInformation("Attempting to retrieve device with cache key: {CacheKey}", cacheKey);

            var cache = await _cachingRepository.GetAsync(cacheKey);

            _logger.LogInformation("Cache value for device ID {Id}: {Cache}", request.DeviceId, cache);

            if (!string.IsNullOrWhiteSpace(cache))
            {
                try
                {
                    getDeviceByIdQuery = JsonSerializer.Deserialize<Device>(cache);
                    _logger.LogInformation("Deserialized device from cache for device ID {Id}", request.DeviceId);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to deserialize device from cache for device ID {Id}", request.DeviceId);
                    _logger.LogError("Cache content: {Cache}", cache);
                }
            }

            if (getDeviceByIdQuery is null)
            {
                _logger.LogInformation("Device not found in cache, querying database for device ID {Id}", request.DeviceId);
                getDeviceByIdQuery = await _devicesDapperRepository.GetDeviceByIdAsync(request.DeviceId);

                if (getDeviceByIdQuery is null)
                {
                    _logger.LogWarning("Device not found for ID: {Id}", request.DeviceId);
                    throw new InvalidOperationException("Device not found");
                }

                _logger.LogInformation("Device found in database, updating cache for device ID {Id}", request.DeviceId);
                string serializedDevice = JsonSerializer.Serialize(getDeviceByIdQuery);
                _logger.LogInformation("Serialized device for cache: {SerializedDevice}", serializedDevice);

                await _cachingRepository.SetAsync(cacheKey, serializedDevice);
            }

            _logger.LogInformation("GetDeviceByIdQuery handled successfully for device ID: {Id}", request.DeviceId);
            return getDeviceByIdQuery;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDeviceByIdQuery for device ID: {Id}", request.DeviceId);
            throw;
        }
    }
}
