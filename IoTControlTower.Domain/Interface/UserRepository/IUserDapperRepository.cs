using IoTControlTower.Domain.Entities;

public interface IUserDapperRepository
{
    Task<User> GetDeviceById(Guid id);
}
