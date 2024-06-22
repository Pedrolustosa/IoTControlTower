using MediatR;
using IoTControlTower.Domain.Enums;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.CQRS.Devices.Commands;

public abstract class DeviceCommandBase : IRequest<Device>
{
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Url { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastCommunication { get; set; }
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public string? FirmwareVersion { get; set; }

    public DateTime? ManufactureDate { get; set; }
    public string? SerialNumber { get; set; }
    public ConnectionType ConnectionType { get; set; }
    public HealthStatus HealthStatus { get; set; }
    public LastKnownStatus? LastKnownStatus { get; set; }
    public string? Owner { get; set; }
    public DateTime? InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public SensorType? SensorType { get; set; }
    public AlarmSettings? AlarmSettings { get; set; }
    public DateTime? LastHealthCheckDate { get; set; }
    public MaintenanceHistory? MaintenanceHistory { get; set; }
    

    [JsonIgnore]
    public string? Email { get; set; }

    [JsonIgnore]
    public string? UserId { get; set; }
}
