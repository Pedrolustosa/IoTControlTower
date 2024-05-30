using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Devices.Commands;

public abstract class DeviceCommandBase : IRequest<Device>
{
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Url { get; set; }
    public bool? IsActive { get; set; }
}
