using IoTControlTower.Application.DTO.Users;

namespace IoTControlTower.Application.Interface
{
    public interface IUserService
    {
        Task<string> GetUserId();
        Task<bool> GetUserName(string userName);
        Task<UserDTO> GetUserData(UserDTO userDTO);
        Task<UserRegisterDTO> CreateUser(UserRegisterDTO userRegisterDTO);
        Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userRegisterDTO);
    }
}
