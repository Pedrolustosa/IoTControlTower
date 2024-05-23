using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using IoTControlTower.Infra.Context;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;

namespace IoTControlTower.Infra.Repository
{
    public class UserRepository(IoTControlTowerContext context, IHttpContextAccessor httpContextAccessor, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly IoTControlTowerContext _context = context;
        private readonly ILogger<UserRepository> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<User> GetUserData(User user)
        {
            _logger.LogInformation("GetUserData() - Attempting to retrieve data for user: {UserName}", user.UserName);
            try
            {
                var userData = await _context.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (userData == null)
                {
                    _logger.LogWarning("GetUserData() - No data found for user: {UserName}", user.UserName);
                }
                else
                {
                    _logger.LogInformation("GetUserData() - Data retrieved successfully for user: {UserName}", user.UserName);
                }
                return userData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserData() - Error occurred while retrieving data for user: {UserName}", user.UserName);
                throw;
            }
        }

        public async Task<string?> GetUserId()
        {
            _logger.LogInformation("GetUserId() - Attempting to retrieve user ID from context.");
            try
            {
                var name = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
                if (name == null)
                {
                    _logger.LogWarning("GetUserId() - No user name found in context.");
                    return null;
                }

                var user = await _context.Users.FirstOrDefaultAsync(x => x.FullName == name);
                if (user == null)
                {
                    _logger.LogWarning("GetUserId() - No user found with name: {FullName}", name);
                    return null;
                }

                _logger.LogInformation("GetUserId() - User ID retrieved successfully for user: {FullName}", name);
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserId() - Error occurred while retrieving user ID.");
                throw;
            }
        }

        public async Task<bool> GetUserName(string userName)
        {
            _logger.LogInformation("GetUserName() - Attempting to check existence of user: {UserName}", userName);
            try
            {
                var exists = await _context.Users.AnyAsync(x => x.UserName == userName);
                if (exists)
                {
                    _logger.LogInformation("GetUserName() - User found: {UserName}", userName);
                }
                else
                {
                    _logger.LogWarning("GetUserName() - User not found: {UserName}", userName);
                }
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserName() - Error occurred while checking existence of user: {UserName}", userName);
                throw;
            }
        }
    }
}
