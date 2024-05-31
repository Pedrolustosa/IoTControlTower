using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.CQRS.Devices.Queries;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Service
{
    public class DeviceService(IMapper mapper, IMediator mediator, ILogger<DeviceService> logger) : IDeviceService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<DeviceService> _logger = logger;

        public async Task<IEnumerable<DeviceDTO>> GetDevices()
        {
            try
            {
                _logger.LogInformation("GetDevices() - Retrieving devices");

                var devicesQuery = new GetDevicesQuery();

                if (devicesQuery is null)
                {
                    _logger.LogWarning("GetDevices() - Device query is null");
                    throw new ArgumentNullException(nameof(devicesQuery), "Query object cannot be null.");
                }

                var result = await _mediator.Send(devicesQuery);

                if (result is null)
                {
                    _logger.LogWarning("GetDevices() - No devices found");
                    return Enumerable.Empty<DeviceDTO>();
                }

                var deviceDTOs = _mapper.Map<List<DeviceDTO>>(result);
                _logger.LogInformation("GetDevices() - Devices retrieved successfully");
                return deviceDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDevices() - Error retrieving devices");
                throw;
            }
        }


        public async Task<DeviceDTO> GetDeviceById(int? id)
        {
            try
            {
                _logger.LogInformation("GetDeviceById()");

                var devicesByIdQuery = new GetDeviceByIdQuery { Id = id.Value };

                if (devicesByIdQuery is null)
                {
                    _logger.LogWarning("GetDeviceById() - Device by ID query is null");
                    throw new ArgumentNullException(nameof(devicesByIdQuery), "Query object cannot be null.");
                }

                var result = await _mediator.Send(devicesByIdQuery);

                return _mapper.Map<DeviceDTO>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeviceCreateDTO> CreateDevice(DeviceCreateDTO deviceDto)
        {
            try
            {
                _logger.LogInformation("CreateDevice()");

                var createDeviceCommand = _mapper.Map<CreateDeviceCommand>(deviceDto);

                await _mediator.Send(createDeviceCommand);

                return _mapper.Map<DeviceCreateDTO>(createDeviceCommand);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeviceUpdateDTO> UpdateDevice(DeviceUpdateDTO deviceUpdateDto)
        {
            try
            {
                _logger.LogInformation("UpdateDevice()");

                var updateDeviceCommand = _mapper.Map<UpdateDeviceCommand>(deviceUpdateDto);

                await _mediator.Send(updateDeviceCommand);

                return _mapper.Map<DeviceUpdateDTO>(updateDeviceCommand);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDevice(int? id)
        {
            try
            {
                _logger.LogInformation("DeleteDevice()");

                var deleteDeviceCommand = new DeleteDeviceCommand { Id = id.Value };

                await _mediator.Send(deleteDeviceCommand);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
