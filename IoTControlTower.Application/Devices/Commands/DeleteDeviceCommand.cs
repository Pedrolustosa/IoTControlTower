using MediatR;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Validation;
using IoTControlTower.Infra.Repository.DeviceRepository;

namespace IoTControlTower.Application.Devices.Commands;

public class DeleteDeviceCommand : IRequest<Device>
{
    public int Id { get; set; }

    public class DeleteDeviceCommandHandler(IUnitOfWork unitOfWork, ILogger<DevicesEFRepository> logger) : IRequestHandler<DeleteDeviceCommand, Device>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DevicesEFRepository> _logger = logger;

        public async Task<Device> Handle(DeleteDeviceCommand request, CancellationToken CancellationToken)
        {
            try
            {
                _logger.LogInformation("DeleteDeviceCommand", request.Id);
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
}
