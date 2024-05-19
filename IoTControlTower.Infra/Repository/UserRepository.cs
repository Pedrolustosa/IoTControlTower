using IoTControlTower.Domain.Interface;
using IoTControlTower.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IoTControlTower.Infra.Repository
{
    public class UserRepository(IoTControlTowerContext context) : IUserRepository
    {
        private readonly IoTControlTowerContext _context = context;

        public async Task<bool> GetUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }
    }
}
