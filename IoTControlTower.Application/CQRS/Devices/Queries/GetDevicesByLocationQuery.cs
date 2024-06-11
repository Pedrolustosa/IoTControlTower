using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDevicesByLocationQuery : IRequest<IEnumerable<Device>>
{
    public string Location { get; set; }
}
