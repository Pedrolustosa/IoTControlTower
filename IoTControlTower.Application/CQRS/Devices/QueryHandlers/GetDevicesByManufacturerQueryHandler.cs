using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.QueryHandlers;

public class GetDevicesByManufacturerQueryHandler(IDeviceDapperRepository deviceRepository, ILogger<GetDevicesByManufacturerQueryHandler> logger) : IRequestHandler<GetDevicesByManufacturerQuery, IEnumerable<Device>>
{
    private readonly IDeviceDapperRepository _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
    private readonly ILogger<GetDevicesByManufacturerQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<Device>> Handle(GetDevicesByManufacturerQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDevicesByManufacturerQuery for manufacturer: {Manufacturer}", request.Manufacturer);

        try
        {
            var devices = await _deviceRepository.GetDevicesByManufacturer(request.Manufacturer);
            _logger.LogInformation("Successfully retrieved devices for manufacturer: {Manufacturer}", request.Manufacturer);
            return devices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDevicesByManufacturerQuery for manufacturer: {Manufacturer}", request.Manufacturer);
            throw new Exception("An error occurred while handling the GetDevicesByManufacturerQuery.", ex);
        }
    }
}
