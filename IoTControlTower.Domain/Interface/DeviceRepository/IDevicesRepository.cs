using IoTControlTower.Domain.Entities;

public interface IDevicesRepository
{
    Task<Device> GetDeviceById(int id);
    Task<Device> AddDeviceAsync(Device device);
    void UpdateDeviceAsync(Device device);
    Task<Device> DeleteDeviceAsync(int deviceId);
}
