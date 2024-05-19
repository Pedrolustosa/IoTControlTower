using IoTControlTower.Application.DTO;

namespace IoTControlTower.Application.Interface
{
    public interface IUserService
    {
        Task<bool> GetUserName(string userName);
        Task<bool> Post(UserRegisterDTO applicationUserRegisterDTO, string role);
    }
}
