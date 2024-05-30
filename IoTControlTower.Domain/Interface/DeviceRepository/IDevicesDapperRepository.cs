using IoTControlTower.Domain.Entities;

public interface IDevicesDapperRepository
{
    Task<Device> GetDeviceById(int id);
    Task<IEnumerable<Device>> GetDevices();
}
