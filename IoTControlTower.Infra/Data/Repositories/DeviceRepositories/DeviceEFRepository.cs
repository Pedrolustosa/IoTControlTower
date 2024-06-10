using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Context;

namespace IoTControlTower.Infra.Data.Repositories.DeviceRepository;

public class DeviceEFRepository(IoTControlTowerContext ioTControlTowerContext,
                                ILogger<DeviceEFRepository> logger) : IDeviceRepository
{
    private readonly ILogger<DeviceEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IoTControlTowerContext _context = ioTControlTowerContext ?? throw new ArgumentNullException(nameof(ioTControlTowerContext));

    public async Task<Device> GetDeviceByIdAsync(int deviceId)
    {
        _logger.LogInformation("Getting device by ID: {DeviceId}", deviceId);
        var device = await _context.Devices.FindAsync(deviceId);

        if (device is null)
        {
            _logger.LogWarning("Device not found for ID: {DeviceId}", deviceId);
            throw new InvalidOperationException("Device not found");
        }
        return device;
    }

    public async Task<Device> AddDeviceAsync(Device device)
    {
        _logger.LogInformation("Adding new device");
        ArgumentNullException.ThrowIfNull(device);
        await _context.AddAsync(device);
        return device;
    }

    public async Task<Device> DeleteDeviceAsync(int deviceId)
    {
        _logger.LogInformation("Deleting device with ID: {DeviceId}", deviceId);
        var device = await GetDeviceByIdAsync(deviceId);
        _context.Devices.Remove(device);
        return device;
    }

    public void UpdateDeviceAsync(Device device)
    {
        _logger.LogInformation("Updating device with ID: {DeviceId}", device.Id);
        ArgumentNullException.ThrowIfNull(device);
        _context.Devices.Update(device);
    }
}
