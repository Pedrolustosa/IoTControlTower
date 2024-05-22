using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using IoTControlTower.Infra.Context;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Interface;

namespace IoTControlTower.Infra.Repository
{
    public class UserRepository(IoTControlTowerContext context, IHttpContextAccessor httpContextAccessor) : IUserRepository
    {
        private readonly IoTControlTowerContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string?> GetUserId()
        {
            try
            {
                var name = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
                var userId = await _context.Users.FirstOrDefaultAsync(x => x.FullName == name) ?? new();
                return userId.Id;
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
