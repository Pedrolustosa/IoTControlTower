using IoTControlTower.Application.DTO.User;

namespace IoTControlTower.Application.Interface
{
    public interface IUserService
    {
        Task<string> GetUserId();
        Task<bool> GetUserName(string userName);
        Task<UserDTO> GetUserData(UserDTO userDTO);
        
        Task<bool> CreateUser(UserRegisterDTO userRegisterDTO, string role);
        Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userRegisterDTO);
    }
}
