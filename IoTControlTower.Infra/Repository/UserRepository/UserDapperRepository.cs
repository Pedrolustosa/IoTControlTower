using Dapper;
using System.Data;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Infra.Repository.UserRepository;

public class UserDapperRepository(IDbConnection dbConnection) : IUserDapperRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<User> GetDeviceById(Guid id)
    {
        try
        {
            string query = "SELECT * FROM Users WHERE Id = @id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
        }
        catch (Exception)
        {
            throw;
        }
    }
}
