using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interface
{
    public interface IDevicesRepository
    {
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device> GetByIdAsync(int id);

        Task AddAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(Device device);

        Task<DashboardSummary> GetDashboardSummary();
    }
}
