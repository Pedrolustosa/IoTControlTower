using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Interface
{
    public interface IDeviceService
    {
        Task<DeviceDTO> GetDeviceById(int? id);
        Task<IEnumerable<DeviceDTO>> GetDevices();

        Task CreateDevice(DeviceDTO deviceDto);
        Task UpdateDevice(DeviceDTO deviceUpdateDTO);
        Task DeleteDevice(int? id);
    }
}
