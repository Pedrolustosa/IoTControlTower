using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface.UserRepository;

public interface IUserRepository
{
    Task<string> GetUserId();
    Task<User> GetUserById(Guid id);
    Task<bool> GetUserName(string userName);
    Task<User> GetUserData(User user);
}
