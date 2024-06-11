using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Context;

namespace IoTControlTower.Infra.Data.Repositories.DeviceRepository;

public class DeviceEFRepository(IoTControlTowerContext ioTControlTowerContext, ILogger<DeviceEFRepository> logger) : IDeviceRepository
{
    private readonly ILogger<DeviceEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IoTControlTowerContext _context = ioTControlTowerContext ?? throw new ArgumentNullException(nameof(ioTControlTowerContext));

    public async Task<Device> GetDeviceByIdAsync(int deviceId)
    {
        _logger.LogInformation("Getting device by ID: {DeviceId}", deviceId);

        try
        {
            var device = await _context.Devices.FindAsync(deviceId);

            if (device is null)
            {
                _logger.LogWarning("Device not found for ID: {DeviceId}", deviceId);
                throw new InvalidOperationException("Device not found");
            }

            return device;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting device by ID: {DeviceId}", deviceId);
            throw new Exception("An error occurred while getting the device by ID.", ex);
        }
    }

    public async Task<Device> AddDeviceAsync(Device device)
    {
        _logger.LogInformation("Adding new device");

        try
        {
            if (device == null)
            {
                _logger.LogError("Device to add is null");
                throw new ArgumentNullException(nameof(device), "Device cannot be null");
            }

            await _context.AddAsync(device);
            return device;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding a new device");
            throw new Exception("An error occurred while adding a new device.", ex);
        }
    }

    public void UpdateDeviceAsync(Device device)
    {
        _logger.LogInformation("Updating device with ID: {DeviceId}", device.Id);

        try
        {
            if (device == null)
            {
                _logger.LogError("Device to update is null");
                throw new ArgumentNullException(nameof(device), "Device cannot be null");
            }

            _context.Devices.Update(device);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating device with ID: {DeviceId}", device.Id);
            throw new Exception("An error occurred while updating the device.", ex);
        }
    }

    public async Task<Device> DeleteDeviceAsync(int deviceId)
    {
        _logger.LogInformation("Deleting device with ID: {DeviceId}", deviceId);

        try
        {
            var device = await GetDeviceByIdAsync(deviceId);
            _context.Devices.Remove(device);
            return device;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting device with ID: {DeviceId}", deviceId);
            throw new Exception("An error occurred while deleting the device.", ex);
        }
    }
}
