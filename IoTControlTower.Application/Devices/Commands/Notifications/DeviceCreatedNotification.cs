using MediatR;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Devices.Commands.Notifications;

public class DeviceCreatedNotification(Device device) : INotification
{
    public Device Device { get; set; } = device;
}
