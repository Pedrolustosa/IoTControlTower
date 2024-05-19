using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface
{
    public interface IUserRepository
    {
        string GetUserId();
        Task<bool> GetUserName(string userName);
    }
}
