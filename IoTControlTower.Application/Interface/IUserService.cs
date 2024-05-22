using IoTControlTower.Application.DTO;

namespace IoTControlTower.Application.Interface
{
    public interface IUserService
    {
        string GetUserId();
        Task<bool> GetUserName(string userName);
        Task<bool> CreateUser(UserRegisterDTO applicationUserRegisterDTO, string role);
    }
}
