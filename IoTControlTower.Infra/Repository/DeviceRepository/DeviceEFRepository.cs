using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Context;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Repository.DeviceRepository;

public class DeviceEFRepository(IoTControlTowerContext ioTControlTowerContext, ILogger<DeviceEFRepository> logger) : IDeviceRepository
{
    private readonly ILogger<DeviceEFRepository> _logger = logger;
    private readonly IoTControlTowerContext _context = ioTControlTowerContext;

    public async Task<Device> GetDeviceById(int deviceId)
    {
        _logger.LogInformation("GetDeviceById()");
        var device = await _context.Devices.FindAsync(deviceId);
        return device ?? throw new InvalidOperationException("Device not found");
    }

    public async Task<Device> AddDeviceAsync(Device device)
    {
        ArgumentNullException.ThrowIfNull(device);
        await _context.AddAsync(device);
        return device;
    }

    public async Task<Device> DeleteDeviceAsync(int deviceId)
    {
        var device = await GetDeviceById(deviceId) ?? throw new InvalidOperationException("Device not found");
        _context.Devices.Remove(device);
        return device;
    }

    public void UpdateDeviceAsync(Device device)
    {
        ArgumentNullException.ThrowIfNull(device);
        _context.Devices.Update(device);
    }
}
