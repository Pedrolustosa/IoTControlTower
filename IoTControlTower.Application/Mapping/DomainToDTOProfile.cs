using AutoMapper;
using IoTControlTower.Application.DTO;
using IoTControlTower.Domain.Entities;

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
        }
    }
}
