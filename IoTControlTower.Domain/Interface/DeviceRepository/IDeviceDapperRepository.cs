using IoTControlTower.Domain.Entities;

public interface IDeviceDapperRepository
{
    Task<Device> GetDeviceById(int id);
    Task<IEnumerable<Device>> GetDevices();
}
