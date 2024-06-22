using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController(ILogger<DevicesController> logger, 
                               IDeviceService deviceService) : ControllerBase
{
    private readonly ILogger<DevicesController> _logger = logger;
    private readonly IDeviceService _deviceService = deviceService;

    [HttpGet("GetDevices")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetDevices()
    {
        _logger.LogInformation("GetDevices action called");
        try
        {
            var devices = await _deviceService.GetDevices();
            _logger.LogInformation("GetDevices action succeeded");
            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting devices");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpGet("GetDevice/{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetDevice(int id)
    {
        _logger.LogInformation("GetDevice action called with id: {Id}", id);
        try
        {
            var device = await _deviceService.GetDeviceById(id);
            if (device is not null)
            {
                _logger.LogInformation("Device found with id: {Id}", id);
                return Ok(device);
            }
            else
            {
                _logger.LogWarning("Device not found with id: {Id}", id);
                return NotFound("Device not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting device with id: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost("CreateDevice")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> CreateDevice([FromBody] DeviceDTO deviceDTO)
    {
        _logger.LogInformation("CreateDevice action called");
        try
        {
            deviceDTO.Email = User?.Claims?.FirstOrDefault()?.Value ?? string.Empty;
            if (deviceDTO.Email is null)
            {
                _logger.LogError("Don't exist User!");
                return BadRequest();
            }
            await _deviceService.CreateDevice(deviceDTO);
            _logger.LogInformation("Device created successfully");
            return Ok(deviceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating device");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPut("UpdateDevice/{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDTO)
    {
        _logger.LogInformation("UpdateDevice action called with id: {Id}", id);
        try
        {
            if (id != deviceDTO.Id)
            {
                _logger.LogWarning("Device ID in the URL does not match the device ID in the body");
                return BadRequest("Device ID mismatch");
            }

            if (deviceDTO is null)
            {
                _logger.LogWarning("Device data is null");
                return BadRequest("Invalid device data");
            }

            deviceDTO.Email = User?.Claims?.FirstOrDefault()?.Value ?? string.Empty;
            if (deviceDTO.Email is null)
            {
                _logger.LogError("Don't exist User!");
                return BadRequest();
            }

            await _deviceService.UpdateDevice(deviceDTO);
            _logger.LogInformation("Device updated successfully with id: {Id}", id);
            return Ok(deviceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating device with id: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete("DeleteDevice/{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        _logger.LogInformation("DeleteDevice action called with id: {Id}", id);
        try
        {
            var device = await _deviceService.GetDeviceById(id);
            if (device is null)
            {
                _logger.LogWarning("Device not found with id: {Id}", id);
                return NotFound("Device not found");
            }

            await _deviceService.DeleteDevice(id);
            _logger.LogInformation("Device deleted successfully with id: {Id}", id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting device with id: {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}
