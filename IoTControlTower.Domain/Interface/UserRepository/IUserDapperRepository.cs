using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface.UserRepository
{
    public interface IUserDapperRepository
    {
        Task<User> GetDeviceById(Guid id);
    }
}
