using IoTControlTower.Domain.Entities;

public interface IDeviceRepository
{
    Task<Device> GetDeviceByIdAsync(int id);
    Task<Device> AddDeviceAsync(Device device);
    void UpdateDeviceAsync(Device device);
    Task<Device> DeleteDeviceAsync(int deviceId);
}
