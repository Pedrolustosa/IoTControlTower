using MediatR;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.CQRS.Devices.Queries;

namespace IoTControlTower.Application.CQRS.Devices.Handlers;

public class GetDeviceByIdQueryHandler(IDeviceDapperRepository devicesDapperRepository) : IRequestHandler<GetDeviceByIdQuery, Device>
{
    private readonly IDeviceDapperRepository _devicesDapperRepository = devicesDapperRepository;

    public async Task<Device> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _devicesDapperRepository.GetDeviceById(request.Id);
            return devices;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
