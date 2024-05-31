namespace IoTControlTower.Application.CQRS.Devices.Commands;

public class UpdateDeviceCommand : DeviceCommandBase
{
    public int Id { get; set; }
}
