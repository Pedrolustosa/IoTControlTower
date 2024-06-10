using AutoMapper;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.CQRS.Users.Commands;

namespace IoTControlTower.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserDTO, UpdateUserCommand>().ReverseMap();
        }
    }
}
