using IoTControlTower.Domain.Entities;

public interface IDeviceDapperRepository
{
    Task<Device> GetDeviceByIdAsync(int id);
    Task<IEnumerable<Device>> GetDevicesAsync();
}
