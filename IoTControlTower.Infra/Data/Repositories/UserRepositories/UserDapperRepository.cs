using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Data.Repositories.UserRepository;

public class UserDapperRepository(IDbConnection dbConnection, ILogger<UserDapperRepository> logger) : IUserDapperRepository
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    private readonly ILogger<UserDapperRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<User> GetUserByEmail(string email)
    {
        _logger.LogInformation("Getting user by email: {Email}", email);

        try
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Email is null or empty");
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            string query = "SELECT UserId as Id * FROM AspNetUsers WHERE Email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument exception occurred while getting user by email: {Email}", email);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user by email: {Email}", email);
            throw new Exception("An error occurred while getting the user by email.", ex);
        }
    }
}
