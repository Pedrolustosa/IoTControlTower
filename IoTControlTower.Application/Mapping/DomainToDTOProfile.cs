using AutoMapper;
using IoTControlTower.Application.DTO.Device;
using IoTControlTower.Application.Devices.Queries;
using IoTControlTower.Application.Devices.Commands;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.Users.Commands;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Mapping
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<DeviceDTO, GetDevicesQuery>().ReverseMap();
            CreateMap<DeviceDTO, GetDeviceByIdQuery>().ReverseMap();
            CreateMap<DeviceCreateDTO, CreateDeviceCommand>().ReverseMap();
            CreateMap<DeviceUpdateDTO, UpdateDeviceCommand>().ReverseMap();

            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserUpdateDTO, UpdateUserCommand>().ReverseMap();
        }
    }
}
