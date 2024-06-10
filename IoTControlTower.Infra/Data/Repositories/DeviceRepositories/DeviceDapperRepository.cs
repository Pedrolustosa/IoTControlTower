using Dapper;
using System.Data;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Data.Repositories.DeviceRepository;

public class DeviceDapperRepository(IDbConnection dbConnection) : IDeviceDapperRepository
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public async Task<Device> GetDeviceByIdAsync(int id)
    {
        try
        {
            string query = "SELECT DeviceId as Id, * FROM Devices WHERE DeviceId = @id";
            var device = await _dbConnection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
            return device;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetDeviceByIdAsync: {ex.Message}");
            throw;
        }
        finally
        {
            if (_dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();
        }
    }

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        try
        {
            string query = "SELECT DeviceId as Id, * FROM Devices";
            return await _dbConnection.QueryAsync<Device>(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetDevicesAsync: {ex.Message}");
            throw;
        }
        finally
        {
            if (_dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();
        }
    }
}
