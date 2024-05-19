using IoTControlTower.Application.DTO;
using IoTControlTower.Application.Interface;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IoTControlTower.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController(IDeviceService deviceService) : ControllerBase
    {
        private readonly IDeviceService _deviceService = deviceService;

        [HttpGet("GetDevices")]
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

        [HttpPost("CreateDevice")]
        public async Task<ActionResult> CreateDevice(DeviceDTO deviceDTO)
        {
            try
            {
                var device = await _deviceService.CreateDevice(deviceDTO);
                return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateDevice/{id}")]
        public async Task<IActionResult> UpdateDevice(int id, [FromBody] DeviceDTO deviceDTO)
        {
            try
            {
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
        public async Task<IActionResult> DeleteDevice(int id)
        {
            try
            {
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
