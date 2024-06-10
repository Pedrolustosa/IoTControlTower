using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface.UnitOfWork;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Domain.Interface.CachingRepository;
using IoTControlTower.Infra.Data.Repositories.DeviceRepository;

namespace IoTControlTower.Application.CQRS.Devices.CommandHandlers;

public class DeleteDeviceCommandHandler(IUnitOfWork unitOfWork,
                                        ILogger<DeviceEFRepository> logger,
                                        ICachingRepository cachingRepository) : IRequestHandler<DeleteDeviceCommand, Device>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<DeviceEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ICachingRepository _cachingRepository = cachingRepository ?? throw new ArgumentNullException(nameof(cachingRepository));

    public async Task<Device> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteDeviceCommand for device ID: {Id}", request.Id);

        try
        {
            _logger.LogInformation("Fetching device for ID: {Id}", request.Id);
            var deletedDevice = await _unitOfWork.DevicesRepository.DeleteDeviceAsync(request.Id);

            if (deletedDevice is null)
            {
                _logger.LogWarning("Device not found for ID: {Id}", request.Id);
                throw new InvalidOperationException("Device not found");
            }

            _logger.LogInformation("Removing device data from cache for ID: {Id}", request.Id);
            await _cachingRepository.RemoveAsync(request.Id.ToString());

            _logger.LogInformation("Committing device deletion for ID: {Id}", request.Id);
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
