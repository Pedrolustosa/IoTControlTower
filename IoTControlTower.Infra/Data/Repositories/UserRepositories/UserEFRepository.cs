using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Data.Context;
using IoTControlTower.Domain.Interface.UserRepository;

namespace IoTControlTower.Infra.Data.Repositories.UserRepository;

public class UserEFRepository(IoTControlTowerContext context, ILogger<UserEFRepository> logger) : IUserRepository
{
    private readonly IoTControlTowerContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<UserEFRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            _logger.LogInformation("GetUserByEmail() - Initialize");

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Email is null or empty");
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                return null;
            }

            _logger.LogInformation("GetUserByEmail() - Succeeded");
            return user;
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
