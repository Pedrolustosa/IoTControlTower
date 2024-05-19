using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface
{
    public interface IUserRepository
    {
        Task<bool> GetUserName(string userName);
    }
}
