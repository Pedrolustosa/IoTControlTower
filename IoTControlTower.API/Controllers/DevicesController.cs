using Microsoft.AspNetCore.Mvc;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController(IDeviceService deviceService, IUserService userService, ILogger<DevicesController> logger) : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IDeviceService _deviceService = deviceService;

        [HttpGet("GetDevices")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            _logger.LogInformation("Executing GetDevices()");

            try
            {
                var devices = await _deviceService.GetDevices();
                _logger.LogInformation("Successfully retrieved {Count} devices.", devices.Count());
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving devices.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetDeviceById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            _logger.LogInformation("Executing GetDeviceById({Id})", id);

            try
            {
                var device = await _deviceService.GetDeviceById(id);
                if (device == null)
                {
                    _logger.LogWarning("Device with ID {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved device with ID {Id}.", id);
                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving device with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetDashboardSummary")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<DashboardSummary>> GetDashboardSummary()
        {
            _logger.LogInformation("Executing GetDashboardSummary()");

            try
            {
                var dashboardSummary = await _deviceService.GetDashboardSummary();
                if (dashboardSummary == null)
                {
                    _logger.LogWarning("Dashboard summary not found.");
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved dashboard summary.");
                return Ok(dashboardSummary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dashboard summary.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateDevice")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> CreateDevice(DeviceDTO deviceDTO)
        {
            _logger.LogInformation("Executing CreateDevice()");

            try
            {
                var createdDevice = await _deviceService.CreateDevice(deviceDTO);

                _logger.LogInformation("Successfully created device with ID {Id} for user {UserId}.", createdDevice.Id, deviceDTO.UserId);
                return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id }, createdDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating device.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateDevice/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceUpdateDTO deviceUpdateDTO)
        {
            _logger.LogInformation("Executing UpdateDevice({Id})", id);

            try
            {
                var userId = await _userService.GetUserId();
                var device = await _deviceService.GetDeviceById(id);

                if (device == null)
                {
                    _logger.LogWarning("Device with ID {Id} not found.", id);
                    return NotFound();
                }

                if (device.UserId != userId)
                {
                    _logger.LogWarning("User {UserId} is not authorized to update device with ID {Id}.", userId, id);
                    return Forbid();
                }

                if (id != deviceUpdateDTO.Id)
                {
                    _logger.LogWarning("ID mismatch: URL ID {UrlId}, DTO ID {DtoId}.", id, deviceUpdateDTO.Id);
                    return BadRequest();
                }

                await _deviceService.UpdateDevice(deviceUpdateDTO);
                _logger.LogInformation("Successfully updated device with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating device with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("DeleteDevice/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            _logger.LogInformation("Executing DeleteDevice({Id})", id);

            try
            {
                var userId = await _userService.GetUserId();
                var device = await _deviceService.GetDeviceById(id);

                if (device == null)
                {
                    _logger.LogWarning("Device with ID {Id} not found.", id);
                    return NotFound();
                }

                if (device.UserId != userId)
                {
                    _logger.LogWarning("User {UserId} is not authorized to delete device with ID {Id}.", userId, id);
                    return Forbid();
                }

                var success = await _deviceService.DeleteDevice(id);

                if (!success)
                {
                    _logger.LogWarning("Failed to delete device with ID {Id}.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully deleted device with ID {Id}.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting device with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
