using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Context;
using IoTControlTower.Domain.Interface.UnitOfWork;
using IoTControlTower.Domain.Interface.UserRepository;
using IoTControlTower.Infra.Repository.UserRepository;
using IoTControlTower.Infra.Repository.DeviceRepository;

namespace IoTControlTower.Infra.Repository;

public class UnitOfWork(IoTControlTowerContext ioTControlTowerContext, 
                        ILogger<UserEFRepository> logger, 
                        ILogger<DeviceEFRepository> loggerEF) : IUnitOfWork, IDisposable
{
    private IUserRepository _userRepository;
    private IDeviceRepository _devicesRepository;

    private readonly ILogger<UserEFRepository> _logger = logger;
    private readonly ILogger<DeviceEFRepository> _loggerEF = loggerEF;
    private readonly IoTControlTowerContext _controlTowerContext = ioTControlTowerContext;

    public IDeviceRepository DevicesRepository 
    {
        get 
        {
            return _devicesRepository ??= new DeviceEFRepository(_controlTowerContext, _loggerEF);
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository ??= new UserEFRepository(_controlTowerContext, _logger);
        }
    }

    public async Task CommitAsync()
    {
        await _controlTowerContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _controlTowerContext.Dispose();
    }
}
