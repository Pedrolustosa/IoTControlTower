using IoTControlTower.Domain.Enums;
using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.Device;

public record DeviceDTO(int Id, string? Description, string? Manufacturer, string? Url, bool IsActive,
                        DateTime? LastCommunication, string? IpAddress, string? Location, string? FirmwareVersion,
                        DateTime? ManufactureDate, string? SerialNumber, ConnectionType? ConnectionType,
                        HealthStatus? HealthStatus, LastKnownStatus? LastKnownStatus, string? Owner, DateTime? InstallationDate,
                        DateTime? LastMaintenanceDate, SensorType? SensorType, AlarmSettings? AlarmSettings,
                        DateTime? LastHealthCheckDate, MaintenanceHistory? MaintenanceHistory)
{
    [JsonIgnore]
    public string? Email { get; set; }

    [JsonIgnore]
    public string? UserId { get; set; }
};