using AutoMapper;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.DTO;
using IoTControlTower.Application.DTO.User;
using IoTControlTower.Application.DTO.Device;

namespace IoTControlTower.Application.Mapping
{
    public class DomainToDTOProfile : Profile
    {
        public DomainToDTOProfile()
        {
            CreateMap<Device, DeviceDTO>().ReverseMap();
            CreateMap<Command, CommandDTO>().ReverseMap();
            CreateMap<CommandDescription, CommandDescriptionDTO>().ReverseMap();
            CreateMap<Parameter, ParameterDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<UserDTO, UserUpdateDTO>().ReverseMap();
        }
    }
}
