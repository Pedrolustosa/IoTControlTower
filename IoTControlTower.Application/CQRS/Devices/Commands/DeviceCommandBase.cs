using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Commands;

public abstract class DeviceCommandBase : IRequest<Device>
{
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Url { get; set; }
    public bool? IsActive { get; set; }
    public string? Email { get; set; }
    public DateTime? LastCommunication { get; private set; }
    public string? IpAddress { get; private set; }
    public string? Location { get; private set; }
    public string? FirmwareVersion { get; private set; }
}
