using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Notifications;

public class DeviceCreatedNotification(Device device) : INotification
{
    public Device Device { get; set; } = device;
}
