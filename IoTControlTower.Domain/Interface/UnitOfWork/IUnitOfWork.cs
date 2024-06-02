using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Domain.Interface.UnitOfWork;

public interface IUnitOfWork
{
    IDeviceRepository DevicesRepository { get; }
    IUserRepository UserRepository { get; }

    Task CommitAsync();
}
