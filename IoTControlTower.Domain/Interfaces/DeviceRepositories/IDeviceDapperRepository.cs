using IoTControlTower.Domain.Entities;

public interface IDeviceDapperRepository
{
    Task<Device> GetDeviceByIdAsync(int id);
    Task<IEnumerable<Device>> GetDevicesAsync();
    Task<IEnumerable<Device>> GetDevicesByLocation(string location);
    Task<IEnumerable<Device>> GetDevicesByManufacturer(string manufacturer);
    Task<IEnumerable<Device>> GetDevicesWithMaintenanceHistory();
    Task<IEnumerable<Device>> GetDevicesWithLastHealthCheckDateOverdue();
}
