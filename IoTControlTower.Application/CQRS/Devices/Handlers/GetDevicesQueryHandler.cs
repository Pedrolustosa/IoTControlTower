using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDevicesQueryHandler(IDeviceDapperRepository devicesDapperRepository, 
                                    ILogger<GetDevicesQueryHandler> logger) : IRequestHandler<GetDevicesQuery, IEnumerable<Device>>
{
    private readonly ILogger<GetDevicesQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository ?? throw new ArgumentNullException(nameof(devicesDapperRepository));

    public async Task<IEnumerable<Device>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesQuery");

        try
        {
            var devices = await _devicesDapperRepository.GetDevicesAsync();
            if (devices is null)
            {
                _logger.LogWarning("No devices found");
                return new List<Device>();
            }

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
