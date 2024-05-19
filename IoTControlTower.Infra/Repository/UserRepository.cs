using IoTControlTower.Domain.Interface;
using IoTControlTower.Infra.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IoTControlTower.Infra.Repository
{
    public class UserRepository(IoTControlTowerContext context, IHttpContextAccessor httpContextAccessor) : IUserRepository
    {
        private readonly IoTControlTowerContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? GetUserId()
        {
            try
            {
                return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetUserName(string userName)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.UserName == userName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
