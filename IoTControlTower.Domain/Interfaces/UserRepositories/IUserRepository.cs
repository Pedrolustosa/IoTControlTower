using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface.UserRepository;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
}
