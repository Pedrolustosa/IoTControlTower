using AutoMapper;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Mapping
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceDTO>().ReverseMap();
            CreateMap<DeviceCreateDTO, CreateDeviceCommand>().ReverseMap();
            CreateMap<DeviceUpdateDTO, UpdateDeviceCommand>().ReverseMap();
        }
    }
}
