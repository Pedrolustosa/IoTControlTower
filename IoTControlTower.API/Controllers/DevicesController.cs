using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DevicesController(ILogger<DevicesController> logger, IDeviceService deviceService) : ControllerBase
    {
        private readonly IDeviceService _deviceService = deviceService;
        private readonly ILogger<DevicesController> _logger = logger;

        [HttpGet("GetDevices")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetDevices()
        {
            _logger.LogInformation("Executing GetDevices()");

            try
            {
                var devices = await _deviceService.GetDevices();
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving devices.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetDevice")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetDevice(int id)
        {
            _logger.LogInformation("Executing GetDeviceById({Id})", id);

            try
            {
                var device = await _deviceService.GetDeviceById(id);
                _logger.LogInformation("Successfully retrieved device with ID {Id}.", id);
                return device != null ? Ok(device) : NotFound("Device not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving device with ID {Id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateDevice")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateDevice([FromBody] DeviceCreateDTO deviceCreateDTO)
        {
            _logger.LogInformation("Executing CreateDevice()");

            try
            {
                var createdDevice = await _deviceService.CreateDevice(deviceCreateDTO);
                _logger.LogInformation("Successfully created device with ID {Id} for user {UserId}.", createdDevice);
                return Ok(createdDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating device.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateDevice/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateDevice(DeviceUpdateDTO deviceUpdateDTO)
        {
            _logger.LogInformation("Executing UpdateDevice({Id})", deviceUpdateDTO.Id);
            try
            {
                var updatedDevice = await _deviceService.UpdateDevice(deviceUpdateDTO);
                _logger.LogInformation("Successfully updated device with ID {Id}.", updatedDevice.Id);
                return updatedDevice != null ? Ok(updatedDevice) : NotFound("Device not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating device with ID {Id}.", deviceUpdateDTO.Id);
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
                var deletedDevice = await _deviceService.DeleteDevice(id);
                return deletedDevice == false ? NotFound("Device not found") : Ok(deletedDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting device with Id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error {id}");
            }
        }
    }
}
