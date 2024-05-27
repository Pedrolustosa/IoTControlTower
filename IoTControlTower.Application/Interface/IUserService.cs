using Microsoft.AspNetCore.Mvc;
using IoTControlTower.Application.DTO.User;

namespace IoTControlTower.Application.Interface
{
    public interface IUserService
    {
        Task<string> GetUserId();
        Task<bool> GetUserName(string userName);
        Task<UserDTO> GetUserData(UserDTO userDTO);
        Task<string> ConfirmEmail(string token, string email);
        Task<bool> CreateUser(UserRegisterDTO userRegisterDTO, string role, IUrlHelper urlHelper, string scheme);
        Task<UserUpdateDTO> UpdateUser(UserUpdateDTO userRegisterDTO);
    }
}
