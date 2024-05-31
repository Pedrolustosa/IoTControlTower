using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Validation;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Infra.Repository.DeviceRepository;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class DeleteDeviceCommandHandler(IUnitOfWork unitOfWork, 
                                        ILogger<DeviceEFRepository> logger) : IRequestHandler<DeleteDeviceCommand, Device>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeviceEFRepository> _logger = logger;

    public async Task<Device> Handle(DeleteDeviceCommand request, CancellationToken CancellationToken)
    {
        try
        {
            _logger.LogInformation("DeleteDeviceCommand()  - Id: {Id}", request.Id);
            var deletedDevice = await _unitOfWork.DevicesRepository.DeleteDeviceAsync(request.Id) ?? throw new InvalidOperationException("Device not found");
            await _unitOfWork.CommitAsync();
            return deletedDevice;
        }
        catch (Exception)
        {
            throw;
        }

    }
}
