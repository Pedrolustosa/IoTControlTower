using IoTControlTower.Application.DTO;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Interface
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDTO>> GetDevices();
        Task<DeviceDTO> GetDeviceById(int id);
        Task<DeviceDTO> CreateDevice(DeviceDTO deviceDto);
        Task<bool> UpdateDevice(DeviceUpdateDTO deviceUpdateDTO);
        Task<bool> DeleteDevice(int id);
        Task<DashboardSummaryDTO> GetDashboardSummary();
    }
}
