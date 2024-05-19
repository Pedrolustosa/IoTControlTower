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
                                         .FirstOrDefaultAsync(d => d.Id == id) ?? new();
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

        public async Task<DashboardSummary> GetDashboardSummary()
        {
            try
            {
                var totalDevices = await _context.Devices.CountAsync();
                var activeDevices = await _context.Devices.CountAsync(d => d.IsActive);
                var inactiveDevices = totalDevices - activeDevices;
                var totalUsers = await _context.Users.CountAsync();
                var monitoringDevices = await _context.Devices.CountAsync(d => d.UserId != null);
                return new DashboardSummary
                {
                    TotalDevices = totalDevices,
                    ActiveDevices = activeDevices,
                    InactiveDevices = inactiveDevices,
                    TotalUsers = totalUsers,
                    MonitoringDevices = monitoringDevices
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
