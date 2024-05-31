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
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, AuthenticateDTO>().ReverseMap();
            CreateMap<UserDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserDTO, AuthenticateDTO>().ReverseMap();
            CreateMap<AuthenticateDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserUpdateDTO, UpdateUserCommand>().ReverseMap();
        }
    }
}
