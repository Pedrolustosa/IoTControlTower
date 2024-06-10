using Dapper;
using System.Data;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Data.Repositories.UserRepository;

public class UserDapperRepository(IDbConnection dbConnection) : IUserDapperRepository
{
    private readonly IDbConnection _dbConnection = dbConnection;

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            string query = "SELECT * FROM AspNetUsers WHERE Email = @email";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }
        catch (Exception)
        {
            throw;
        }
    }
}
