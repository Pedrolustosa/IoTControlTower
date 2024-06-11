using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Data.Context;
using IoTControlTower.Domain.Interface.UnitOfWork;
using IoTControlTower.Domain.Interface.UserRepository;
using IoTControlTower.Infra.Data.Repositories.UserRepository;
using IoTControlTower.Infra.Data.Repositories.DeviceRepository;

namespace IoTControlTower.Infra.Data.Repositories;

public class UnitOfWork(IoTControlTowerContext ioTControlTowerContext, ILogger<UserEFRepository> logger, ILogger<DeviceEFRepository> loggerEF) : IUnitOfWork, IDisposable
{
    private readonly IoTControlTowerContext _controlTowerContext = ioTControlTowerContext ?? throw new ArgumentNullException(nameof(ioTControlTowerContext));
    private readonly ILogger<UserEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ILogger<DeviceEFRepository> _loggerEF = loggerEF ?? throw new ArgumentNullException(nameof(loggerEF));
    private IUserRepository _userRepository;
    private IDeviceRepository _devicesRepository;

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
        try
        {
            _logger.LogInformation("CommitAsync() - Initialize");

            await _controlTowerContext.SaveChangesAsync();

            _logger.LogInformation("CommitAsync() - Succeeded");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while committing changes to the database");
            throw new Exception("An error occurred while committing changes to the database.", ex);
        }
    }

    public void Dispose() => _controlTowerContext.Dispose();
}
