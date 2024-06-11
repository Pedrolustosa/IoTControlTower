using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Interface;

public interface IDeviceService
{
    #region Commands
    Task CreateDevice(DeviceDTO deviceDto);
    Task UpdateDevice(DeviceDTO deviceUpdateDTO);
    Task DeleteDevice(int? id);
    #endregion

    #region Queries
    Task<IEnumerable<DeviceDTO>> GetDevices();
    Task<DeviceDTO> GetDeviceById(int? id);
    Task<int> GetTotalDeviceCount();
    Task<int> GetActiveDeviceCount();
    Task<int> GetConnectedViaWiFiCount();
    Task<int> GetConnectedViaEthernetCount();
    Task<int> GetHealthyDeviceCount();
    Task<int> GetUnhealthyDeviceCount();
    Task<int> GetDevicesWithAlarmsCount();
    Task<int> GetDevicesWithoutAlarmsCount();
    Task<IEnumerable<DeviceDTO>> GetDevicesByLocation(string location);
    Task<IEnumerable<DeviceDTO>> GetDevicesByManufacturer(string manufacturer);
    Task<IEnumerable<DeviceDTO>> GetDevicesWithMaintenanceHistory();
    Task<IEnumerable<DeviceDTO>> GetDevicesWithLastHealthCheckDateOverdue();
    #endregion
}