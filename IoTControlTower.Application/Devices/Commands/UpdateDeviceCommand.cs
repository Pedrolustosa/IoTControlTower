using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Repository;
using IoTControlTower.Infra.Repository.DeviceRepository;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Devices.Commands;

public class UpdateDeviceCommand : DeviceCommandBase
{
    public int Id { get; set; }

    public class UpdateDeviceCommandHandler(UnitOfWork unitOfWork, ILogger<DevicesEFRepository> logger) : IRequestHandler<UpdateDeviceCommand, Device>
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DevicesEFRepository> _logger = logger;

        public async Task<Device> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("UpdateDeviceCommand", request.Id);
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
}
