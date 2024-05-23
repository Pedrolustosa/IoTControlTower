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
            try
            {
                return await _context.Devices.Include(d => d.CommandDescriptions)
                                         .ThenInclude(cd => cd.Command)
                                         .FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            try
            {
                return await _context.Devices.Include(d => d.CommandDescriptions)
                                         .ThenInclude(cd => cd.Command)
                                         .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
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
            try
            {
                _context.Devices.Update(device);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(Device device)
        {
            try
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
