using IoTControlTower.Application.DTO;
using IoTControlTower.Domain.Interface;
using IoTControlTower.Application.Interface;
using AutoMapper;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Service
{
    public class DeviceService(IDevicesRepository devicesRepository, IMapper mapper) : IDeviceService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IDevicesRepository _deviceRepository = devicesRepository;

        public async Task<IEnumerable<DeviceDTO>> GetDevices()
        {
            try
            {
                var devices = await _deviceRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<DeviceDTO>>(devices);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DeviceDTO> GetDeviceById(int id)
        {
            try
            {
                var device = await _deviceRepository.GetByIdAsync(id);
                return _mapper.Map<DeviceDTO>(device);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeviceDTO> CreateDevice(DeviceDTO deviceDto)
        {
            try
            {
                var device = _mapper.Map<Device>(deviceDto);
                await _deviceRepository.AddAsync(device);
                return _mapper.Map<DeviceDTO>(device);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeviceDTO> UpdateDevice(DeviceDTO deviceDto)
        {
            try
            {
                var device = _mapper.Map<Device>(deviceDto);
                await _deviceRepository.UpdateAsync(device);
                return _mapper.Map<DeviceDTO>(device);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDevice(int id)
        {
            try
            {
                var device = await _deviceRepository.GetByIdAsync(id);
                if (device == null) return false;
                await _deviceRepository.DeleteAsync(device);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DashboardSummaryDTO> GetDashboardSummary()
        {
            try
            {
                var dashboard = _deviceRepository.GetDashboardSummary();
                return _mapper.Map<DashboardSummaryDTO>(dashboard);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
