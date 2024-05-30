using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Devices.Queries;

public class GetDevicesQuery : IRequest<IEnumerable<Device>>
{
    public class GetDevicesQueryHandler(IDevicesDapperRepository devicesDapperRepository) : IRequestHandler<GetDevicesQuery, IEnumerable<Device>>
    {
        private readonly IDevicesDapperRepository _devicesDapperRepository = devicesDapperRepository;

        public async Task<IEnumerable<Device>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var devices = await _devicesDapperRepository.GetDevices();
                return devices;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
