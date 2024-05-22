using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController(IDeviceService deviceService, IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IDeviceService _deviceService = deviceService;

        [HttpGet("GetDevices")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            try
            {
                var devices = await _deviceService.GetDevices();
                return Ok(devices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetDeviceById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            try
            {
                var device = await _deviceService.GetDeviceById(id);
                if (device == null) return NotFound();
                return Ok(device);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpGet("GetDashboardSummary")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<DashboardSummary>> GetDashboardSummary()
        {
            try
            {
                var dashboardSummary = await _deviceService.GetDashboardSummary();
                if (dashboardSummary == null) return NotFound();
                return Ok(dashboardSummary);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpPost("CreateDevice")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> CreateDevice(DeviceDTO deviceDTO)
        {
            try
            {
                var userId = _userService.GetUserId();
                deviceDTO.UserId = userId;
                var createdDevice = await _deviceService.CreateDevice(deviceDTO);
                return CreatedAtAction(nameof(GetDeviceById), new { id = createdDevice.Id }, createdDevice);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateDevice/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDTO)
        {
            try
            {
                var userId = _userService.GetUserId();
                var device = await _deviceService.GetDeviceById(id);
                if (device == null)  return NotFound();
                if (device.UserId != userId) return Forbid();
                if (id != deviceDTO.Id) return BadRequest();
                await _deviceService.UpdateDevice(deviceDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpDelete("DeleteDevice/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
                var userId = _userService.GetUserId();
                var device = await _deviceService.GetDeviceById(id);
                if (device == null) return NotFound();
                if (device.UserId != userId) return Forbid();
                var success = await _deviceService.DeleteDevice(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }
    }
}
