using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Commands;

public class DeleteDeviceCommand : IRequest<Device>
{
    public int Id { get; set; }
}
