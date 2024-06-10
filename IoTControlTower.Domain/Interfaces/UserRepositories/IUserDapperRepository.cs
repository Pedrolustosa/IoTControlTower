using IoTControlTower.Domain.Entities;

public interface IUserDapperRepository
{
    Task<User> GetUserByEmail(string email);
}
