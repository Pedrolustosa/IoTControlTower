using IoTControlTower.Infra.Context;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;

namespace IoTControlTower.Infra.Repository
{
    public class DevicesRepository(IoTControlTowerContext ioTControlTowerContext) : IDevicesRepository
    {
        private readonly IoTControlTowerContext _context = ioTControlTowerContext;

        public async Task<Device> GetByIdAsync(int id)
        {
            return await _context.Devices.Include(d => d.CommandDescriptions)
                                         .ThenInclude(cd => cd.Command)
                                         .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _context.Devices.Include(d => d.CommandDescriptions)
                                         .ThenInclude(cd => cd.Command)
                                         .ToListAsync();
        }

        public async Task AddAsync(Device device)
        {
            try
            {
                await _context.Devices.AddAsync(device);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Device device)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
    }
}
