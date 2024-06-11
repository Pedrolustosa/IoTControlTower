using AutoMapper;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.DTO.Users;
using IoTControlTower.Application.CQRS.Users.Commands;

namespace IoTControlTower.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        try
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserDTO, UpdateUserCommand>().ReverseMap();
        }
        catch (AutoMapperConfigurationException ex)
        {
            Console.WriteLine($"An AutoMapperConfigurationException occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while configuring mapping profiles: {ex.Message}");
            throw;
        }
    }
}
