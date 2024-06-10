using IoTControlTower.Application.DTO.Users;

namespace IoTControlTower.Application.Interface;

public interface IUserService
{
    Task<UserDTO> GetUserByEmail(string email);
    Task<UserDTO> CreateUser(UserDTO userRegisterDTO);
    Task<UserDTO> UpdateUser(UserDTO userRegisterDTO);
}
