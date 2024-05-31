using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Infra.Repository.DeviceRepository;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class UpdateDeviceCommandHandler(UnitOfWork unitOfWork, 
                                        ILogger<DeviceEFRepository> logger) : IRequestHandler<UpdateDeviceCommand, Device>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeviceEFRepository> _logger = logger;

    public async Task<Device> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("UpdateDeviceCommand() - Id: {Id}", request.Id);
            var existingDevice = await _unitOfWork.DevicesRepository.GetDeviceById(request.Id) ?? throw new InvalidOperationException("Device not found");
            existingDevice.Update(request.Description, request.Manufacturer, request.Url, request.IsActive);
            _unitOfWork.DevicesRepository.UpdateDeviceAsync(existingDevice);
            await _unitOfWork.CommitAsync();
            return existingDevice;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
