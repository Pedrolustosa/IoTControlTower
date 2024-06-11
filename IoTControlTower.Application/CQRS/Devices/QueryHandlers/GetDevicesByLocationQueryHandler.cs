using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.QueryHandlers;

public class GetDevicesByLocationQueryHandler(IDeviceDapperRepository deviceRepository, ILogger<GetDevicesByLocationQueryHandler> logger) : IRequestHandler<GetDevicesByLocationQuery, IEnumerable<Device>>
{
    private readonly IDeviceDapperRepository _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    private readonly ILogger<GetDevicesByLocationQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<Device>> Handle(GetDevicesByLocationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesByLocationQuery for location: {Location}", request.Location);

        try
        {
            var devices = await _deviceRepository.GetDevicesByLocation(request.Location);
            _logger.LogInformation("Successfully retrieved devices for location: {Location}", request.Location);
            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDevicesByLocationQuery for location: {Location}", request.Location);
            throw new Exception("An error occurred while handling the GetDevicesByLocationQuery.", ex);
        }
    }
}
