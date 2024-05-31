using MediatR;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDevicesQueryHandler(IDeviceDapperRepository devicesDapperRepository) : IRequestHandler<GetDevicesQuery, IEnumerable<Device>>
{
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository;

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
