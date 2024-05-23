using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface
{
    public interface IUserRepository
    {
        Task<string> GetUserId();
        Task<bool> GetUserName(string userName);
        Task<User> GetUserData(User user);
    }
}
