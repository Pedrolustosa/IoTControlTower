using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Context;
using IoTControlTower.Domain.Validation;
using IoTControlTower.Infra.Repository.DeviceRepository;
using IoTControlTower.Infra.Repository.UserRepository;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Infra.Repository;

public class UnitOfWork(IoTControlTowerContext ioTControlTowerContext, 
                        ILogger<UserEFRepository> logger, 
                        ILogger<DeviceEFRepository> loggerEF, 
                        IHttpContextAccessor httpContextAccessor) : IUnitOfWork, IDisposable
{
    private IUserRepository _userRepository;
    private IDeviceRepository _devicesRepository;

    private readonly ILogger<UserEFRepository> _logger = logger;
    private readonly ILogger<DeviceEFRepository> _loggerEF = loggerEF;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
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
            return _userRepository ??= new UserEFRepository(_controlTowerContext, _httpContextAccessor, _logger);
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
