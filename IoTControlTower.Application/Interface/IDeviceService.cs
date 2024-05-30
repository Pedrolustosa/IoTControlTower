using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Interface
{
    public interface IDeviceService
    {
        Task<DeviceDTO> GetDeviceById(int? id);
        Task<IEnumerable<DeviceDTO>> GetDevices();

        Task<DeviceCreateDTO> CreateDevice(DeviceCreateDTO deviceDto);
        Task<DeviceUpdateDTO> UpdateDevice(DeviceUpdateDTO deviceUpdateDTO);
        Task<bool> DeleteDevice(int? id);
    }
}
