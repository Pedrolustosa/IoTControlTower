using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Devices.Queries;

public class GetDeviceByIdQuery : IRequest<Device>
{
    public int Id { get; set; }

    public class GetDeviceByIdQueryHandler(IDevicesDapperRepository devicesDapperRepository) : IRequestHandler<GetDeviceByIdQuery, Device>
    {
        private readonly IDevicesDapperRepository _devicesDapperRepository = devicesDapperRepository;

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
}
