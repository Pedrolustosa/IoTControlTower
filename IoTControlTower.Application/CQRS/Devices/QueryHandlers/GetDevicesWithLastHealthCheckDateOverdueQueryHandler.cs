using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.QueryHandlers;

public class GetDevicesWithLastHealthCheckDateOverdueQueryHandler(IDeviceDapperRepository deviceRepository, ILogger<GetDevicesWithLastHealthCheckDateOverdueQueryHandler> logger) : IRequestHandler<GetDevicesWithLastHealthCheckDateOverdueQuery, IEnumerable<Device>>
{
    private readonly IDeviceDapperRepository _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    private readonly ILogger<GetDevicesWithLastHealthCheckDateOverdueQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<Device>> Handle(GetDevicesWithLastHealthCheckDateOverdueQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesWithLastHealthCheckDateOverdueQuery");

        try
        {
            var devices = await _deviceRepository.GetDevicesWithLastHealthCheckDateOverdue();
            _logger.LogInformation("Successfully retrieved devices with last health check date overdue");
            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDevicesWithLastHealthCheckDateOverdueQuery");
            throw new Exception("An error occurred while handling the GetDevicesWithLastHealthCheckDateOverdueQuery.", ex);
        }
    }
}
