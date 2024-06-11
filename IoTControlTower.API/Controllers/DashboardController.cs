using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;

namespace IoTControlTower.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(ILogger<DashboardController> logger, IDeviceService deviceService) : ControllerBase
{
    private readonly ILogger<DashboardController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IDeviceService _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService));

    [HttpGet("Statistics")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetDeviceStatistics()
    {
        _logger.LogInformation("GetDeviceStatistics action called");

        try
        {
            var totalDevices = await _deviceService.GetTotalDeviceCount();
            var healthyDevices = await _deviceService.GetHealthyDeviceCount();
            var unhealthyDevices = await _deviceService.GetUnhealthyDeviceCount();
            var activeDevices = await _deviceService.GetActiveDeviceCount();
            var connectedViaEthernetDevices = await _deviceService.GetConnectedViaEthernetCount();
            var connectedViaWiFiDevices = await _deviceService.GetConnectedViaWiFiCount();
            var devicesWithoutAlarms = await _deviceService.GetDevicesWithoutAlarmsCount();
            var devicesWithAlarms = await _deviceService.GetDevicesWithAlarmsCount();

            var deviceStatistics = new
            {
                TotalDevices = totalDevices,
                HealthyDevices = healthyDevices,
                UnhealthyDevices = unhealthyDevices,
                ActiveDevices = activeDevices,
                ConnectedViaEthernetDevices = connectedViaEthernetDevices,
                ConnectedViaWiFiDevices = connectedViaWiFiDevices,
                DevicesWithoutAlarms = devicesWithoutAlarms,
                DevicesWithAlarms = devicesWithAlarms
            };

            _logger.LogInformation("GetDeviceStatistics action succeeded");
            return Ok(deviceStatistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting device statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("GetDevicesWithLastHealthCheckDateOverdue")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetDevicesWithLastHealthCheckDateOverdue()
    {
        _logger.LogInformation("GetDevicesWithLastHealthCheckDateOverdue action called");

        try
        {
            var devices = await _deviceService.GetDevicesWithLastHealthCheckDateOverdue();
            _logger.LogInformation("GetDevicesWithLastHealthCheckDateOverdue action succeeded");
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices with last health check overdue");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("DevicesWithMaintenanceHistory")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetDevicesWithMaintenanceHistory()
    {
        _logger.LogInformation("GetDevicesWithMaintenanceHistory action called");

        try
        {
            var devices = await _deviceService.GetDevicesWithMaintenanceHistory();
            _logger.LogInformation("GetDevicesWithMaintenanceHistory action succeeded");
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices with maintenance history");
            return StatusCode(500, "Internal server error");
        }
    }
}
