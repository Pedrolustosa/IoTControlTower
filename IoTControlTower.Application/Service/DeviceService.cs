using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.CQRS.Devices.Queries;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Service
{
    public class DeviceService(IMapper mapper, 
                               IMediator mediator, 
                               ILogger<DeviceService> logger) : IDeviceService
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        private readonly ILogger<DeviceService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<IEnumerable<DeviceDTO>> GetDevices()
        {
            _logger.LogInformation("GetDevices method called");

            try
            {
                var devicesQuery = new GetDevicesQuery();
                var result = await _mediator.Send(devicesQuery);

                if (result is null)
                {
                    _logger.LogWarning("No devices found");
                    return Enumerable.Empty<DeviceDTO>();
                }

                var deviceDTOs = _mapper.Map<List<DeviceDTO>>(result);
                _logger.LogInformation("GetDevices method succeeded");
                return deviceDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting devices");
                throw;
            }
        }

        public async Task<DeviceDTO?> GetDeviceById(int? id)
        {
            if (!id.HasValue)
            {
                _logger.LogWarning("GetDeviceById method called with null id");
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            _logger.LogInformation("GetDeviceById method called with id: {Id}", id);

            try
            {

                var devicesByIdQuery = new GetDeviceByIdQuery { DeviceId = id.Value };
                var result = await _mediator.Send(devicesByIdQuery);

                if (result is null)
                {
                    _logger.LogWarning("Device not found with id: {Id}", id);
                    return null;
                }

                var deviceDTO = _mapper.Map<DeviceDTO>(result);
                _logger.LogInformation("GetDeviceById method succeeded for id: {Id}", id);
                return deviceDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting device with id: {Id}", id);
                throw;
            }
        }

        public async Task CreateDevice(DeviceDTO deviceDto)
        {
            if (deviceDto is null)
            {
                _logger.LogWarning("CreateDevice method called with null deviceDto");
                throw new ArgumentNullException(nameof(deviceDto), "DeviceDTO cannot be null");
            }

            _logger.LogInformation("CreateDevice method called");

            try
            {
                var createDeviceCommand = _mapper.Map<CreateDeviceCommand>(deviceDto);
                await _mediator.Send(createDeviceCommand);
                _logger.LogInformation("CreateDevice method succeeded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating device");
                throw;
            }
        }

        public async Task UpdateDevice(DeviceDTO deviceDto)
        {
            if (deviceDto is null)
            {
                _logger.LogWarning("UpdateDevice method called with null deviceUpdateDto");
                throw new ArgumentNullException(nameof(deviceDto), "DeviceDTO cannot be null");
            }

            _logger.LogInformation("UpdateDevice method called for id: {Id}", deviceDto.Id);

            try
            {
                var updateDeviceCommand = _mapper.Map<UpdateDeviceCommand>(deviceDto);
                await _mediator.Send(updateDeviceCommand);
                _logger.LogInformation("UpdateDevice method succeeded for id: {Id}", deviceDto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating device with id: {Id}", deviceDto.Id);
                throw;
            }
        }

        public async Task DeleteDevice(int? id)
        {
            if (!id.HasValue)
            {
                _logger.LogWarning("DeleteDevice method called with null id");
                throw new ArgumentNullException(nameof(id), "ID cannot be null");
            }

            _logger.LogInformation("DeleteDevice method called for id: {Id}", id);

            try
            {
                var deleteDeviceCommand = new DeleteDeviceCommand { Id = id.Value };
                await _mediator.Send(deleteDeviceCommand);
                _logger.LogInformation("DeleteDevice method succeeded for id: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting device with id: {Id}", id);
                throw;
            }
        }
    }
}
