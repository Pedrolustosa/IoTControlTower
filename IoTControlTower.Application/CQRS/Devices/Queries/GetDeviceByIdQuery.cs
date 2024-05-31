using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Queries;

public class GetDeviceByIdQuery : IRequest<Device>
{
    public int Id { get; set; }
}
