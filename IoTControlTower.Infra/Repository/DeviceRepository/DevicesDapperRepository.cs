using Dapper;
using System.Data;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Repository.DeviceRepository;

public class DevicesDapperRepository(IDbConnection dbConnection) : IDevicesDapperRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<Device> GetDeviceById(int id)
    {
        try
        {
            string query = "SELECT * FROM Devices WHERE Id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Device>(query, new { Id = id });
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Device>> GetDevices()
    {
        try
        {
            string query = "SELECT * FROM Devices";
            return await _dbConnection.QueryAsync<Device>(query);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
