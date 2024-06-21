using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Enums;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.CQRS.Devices.Queries;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.Interfaces;
using IoTControlTower.Application.Services;
using System.Text.Json;

namespace IoTControlTower.Application.Service;

public class DeviceService(IMapper mapper,
                           IRabbitMQService rabbitMQService,
                           IMediator mediator,
                           ILogger<DeviceService> logger) : IDeviceService
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IRabbitMQService _rabbitMQService = rabbitMQService ?? throw new ArgumentNullException(nameof(rabbitMQService));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ILogger<DeviceService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    #region Commands
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

            var commandJson = JsonSerializer.Serialize(createDeviceCommand);
            var rabbitMessage = new RabbitMessage
            {
                Id = deviceDto.Id,
                Title = "New Device Created",
                Text = $"A new device with ID {deviceDto.Id} has been created.",
                Payload = commandJson
            };
            _rabbitMQService.SendMessage(rabbitMessage);
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
    #endregion

    #region Queries
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

    public async Task<IEnumerable<DeviceDTO>> GetDevicesByLocation(string location)
    {
        _logger.LogInformation("GetDevicesByLocation method called with location: {Location}", location);

        try
        {
            var devicesQuery = new GetDevicesByLocationQuery { Location = location };
            var result = await _mediator.Send(devicesQuery);

            if (result is null)
            {
                _logger.LogWarning("No devices found for location: {Location}", location);
                return Enumerable.Empty<DeviceDTO>();
            }

            var deviceDTOs = result.Select(device => _mapper.Map<DeviceDTO>(device));
            _logger.LogInformation("GetDevicesByLocation method succeeded for location: {Location}", location);
            return deviceDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices for location: {Location}", location);
            throw;
        }
    }

    public async Task<IEnumerable<DeviceDTO>> GetDevicesByManufacturer(string manufacturer)
    {
        _logger.LogInformation("GetDevicesByManufacturer method called with manufacturer: {Manufacturer}", manufacturer);

        try
        {
            var devicesQuery = new GetDevicesByManufacturerQuery { Manufacturer = manufacturer };
            var result = await _mediator.Send(devicesQuery);

            if (result is null)
            {
                _logger.LogWarning("No devices found for manufacturer: {Manufacturer}", manufacturer);
                return Enumerable.Empty<DeviceDTO>();
            }

            var deviceDTOs = result.Select(device => _mapper.Map<DeviceDTO>(device));
            _logger.LogInformation("GetDevicesByManufacturer method succeeded for manufacturer: {Manufacturer}", manufacturer);
            return deviceDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices for manufacturer: {Manufacturer}", manufacturer);
            throw;
        }
    }

    public async Task<IEnumerable<DeviceDTO>> GetDevicesWithMaintenanceHistory()
    {
        _logger.LogInformation("GetDevicesWithMaintenanceHistory method called");

        try
        {
            var devicesQuery = new GetDevicesWithMaintenanceHistoryQuery();
            var result = await _mediator.Send(devicesQuery);

            if (result is null)
            {
                _logger.LogWarning("No devices found with maintenance history");
                return Enumerable.Empty<DeviceDTO>();
            }

            var deviceDTOs = result.Select(device => _mapper.Map<DeviceDTO>(device));
            _logger.LogInformation("GetDevicesWithMaintenanceHistory method succeeded");
            return deviceDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices with maintenance history");
            throw;
        }
    }

    public async Task<IEnumerable<DeviceDTO>> GetDevicesWithLastHealthCheckDateOverdue()
    {
        _logger.LogInformation("GetDevicesWithLastHealthCheckDateOverdue method called");

        try
        {
            var devicesQuery = new GetDevicesWithLastHealthCheckDateOverdueQuery();
            var result = await _mediator.Send(devicesQuery);

            if (result is null)
            {
                _logger.LogWarning("No devices found with overdue health check");
                return Enumerable.Empty<DeviceDTO>();
            }

            var deviceDTOs = result.Select(device => _mapper.Map<DeviceDTO>(device));
            _logger.LogInformation("GetDevicesWithLastHealthCheckDateOverdue method succeeded");
            return deviceDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices with overdue health check");
            throw;
        }
    }

    public async Task<int> GetUnhealthyDeviceCount()
    {
        _logger.LogInformation("GetUnhealthyDeviceCount method called");

        try
        {
            var unhealthyDevices = await GetDevices();
            return unhealthyDevices.Count(x => x.HealthStatus is HealthStatus.Unhealthy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting unhealthy device count");
            throw;
        }
    }

    public async Task<int> GetDevicesWithAlarmsCount()
    {
        _logger.LogInformation("GetDevicesWithAlarmsCount method called");

        try
        {
            var devicesWithAlarms = await GetDevices();
            return devicesWithAlarms.Count(x => x.AlarmSettings is not null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices with alarms count");
            throw;
        }
    }

    public async Task<int> GetDevicesWithoutAlarmsCount()
    {
        _logger.LogInformation("GetDevicesWithoutAlarmsCount method called");

        try
        {
            var devicesWithoutAlarms = await GetDevices();
            return devicesWithoutAlarms.Count(x => x.AlarmSettings is null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices without alarms count");
            throw;
        }
    }

    public async Task<int> GetTotalDeviceCount()
    {
        _logger.LogInformation("GetTotalDeviceCount method called");

        try
        {
            var devices = await GetDevices();
            return devices.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting total device count");
            throw;
        }
    }

    public async Task<int> GetActiveDeviceCount()
    {
        _logger.LogInformation("GetActiveDeviceCount method called");

        try
        {
            var devices = await GetDevices();
            return devices.Count(device => device.IsActive == true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting active device count");
            throw;
        }
    }

    public async Task<int> GetConnectedViaWiFiCount()
    {
        _logger.LogInformation("GetConnectedViaWiFiCount method called");

        try
        {
            var devices = await GetDevices();
            return devices.Count(device => device.ConnectionType == ConnectionType.WiFi);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices connected via WiFi count");
            throw;
        }
    }

    public async Task<int> GetConnectedViaEthernetCount()
    {
        _logger.LogInformation("GetConnectedViaEthernetCount method called");

        try
        {
            var devices = await GetDevices();
            return devices.Count(device => device.ConnectionType == ConnectionType.Ethernet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices connected via Ethernet count");
            throw;
        }
    }

    public async Task<int> GetHealthyDeviceCount()
    {
        _logger.LogInformation("GetHealthyDeviceCount method called");

        try
        {
            var devices = await GetDevices();
            return devices.Count(device => device.HealthStatus == HealthStatus.Healthy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting healthy device count");
            throw;
        }
    }
    #endregion
}
