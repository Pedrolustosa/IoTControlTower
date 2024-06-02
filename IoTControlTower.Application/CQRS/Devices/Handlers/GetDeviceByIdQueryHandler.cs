using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDeviceByIdQueryHandler(IDeviceDapperRepository devicesDapperRepository,
                                       ILogger<GetDeviceByIdQueryHandler> logger) : IRequestHandler<GetDeviceByIdQuery, Device>
{
    private readonly ILogger<GetDeviceByIdQueryHandler> _logger = logger;
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository;

    public async Task<Device> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetDeviceByIdQuery for device ID: {Id}", request.Id);

        try
        {
            var device = await _devicesDapperRepository.GetDeviceByIdAsync(request.Id);

            if (device is null)
            {
                _logger.LogWarning("Device not found for ID: {Id}", request.Id);
                throw new InvalidOperationException("Device not found");
            }

            _logger.LogInformation("GetDeviceByIdQuery handled successfully for device ID: {Id}", request.Id);
            return device;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetDeviceByIdQuery for device ID: {Id}", request.Id);
            throw;
        }
    }
}
