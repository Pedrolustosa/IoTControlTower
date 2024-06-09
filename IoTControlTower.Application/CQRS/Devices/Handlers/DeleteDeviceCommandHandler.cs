using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface.UnitOfWork;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Infra.Data.Repositories.DeviceRepository;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class DeleteDeviceCommandHandler(IUnitOfWork unitOfWork, 
                                        ILogger<DeviceEFRepository> logger) : IRequestHandler<DeleteDeviceCommand, Device>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<DeviceEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Device> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteDeviceCommand for device ID: {Id}", request.Id);

        try
        {
            var deletedDevice = await _unitOfWork.DevicesRepository.DeleteDeviceAsync(request.Id);

            if (deletedDevice is null)
            {
                _logger.LogWarning("Device not found for ID: {Id}", request.Id);
                throw new InvalidOperationException("Device not found");
            }

            await _unitOfWork.CommitAsync();

            _logger.LogInformation("DeleteDeviceCommand handled successfully for device ID: {Id}", request.Id);
            return deletedDevice;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operation failed for device ID: {Id}", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling DeleteDeviceCommand for device ID: {Id}", request.Id);
            throw;
        }
    }
}
