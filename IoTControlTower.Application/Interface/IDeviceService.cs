using IoTControlTower.Application.DTO;

namespace IoTControlTower.Application.Interface
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDTO>> GetDevices();
        Task<DeviceDTO> GetDeviceById(int id);
        Task<DeviceDTO> CreateDevice(DeviceDTO deviceDto);
        Task<DeviceDTO> UpdateDevice(DeviceDTO deviceDto);
        Task<bool> DeleteDevice(int id);
        Task<DashboardSummaryDTO> GetDashboardSummary();
    }
}
