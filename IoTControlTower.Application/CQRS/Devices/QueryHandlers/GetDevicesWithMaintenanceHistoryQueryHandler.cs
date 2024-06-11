using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.QueryHandlers;

public class GetDevicesWithMaintenanceHistoryQueryHandler(IDeviceDapperRepository deviceRepository, ILogger<GetDevicesWithMaintenanceHistoryQueryHandler> logger) : IRequestHandler<GetDevicesWithMaintenanceHistoryQuery, IEnumerable<Device>>
{
    private readonly IDeviceDapperRepository _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    private readonly ILogger<GetDevicesWithMaintenanceHistoryQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<Device>> Handle(GetDevicesWithMaintenanceHistoryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesWithMaintenanceHistoryQuery");

        try
        {
            var devices = await _deviceRepository.GetDevicesWithMaintenanceHistory();
            _logger.LogInformation("Successfully retrieved devices with maintenance history");
            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDevicesWithMaintenanceHistoryQuery");
            throw new Exception("An error occurred while handling the GetDevicesWithMaintenanceHistoryQuery.", ex);
        }
    }
}
