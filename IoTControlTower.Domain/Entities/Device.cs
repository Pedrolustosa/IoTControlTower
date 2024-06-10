using IoTControlTower.Domain.Enums;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControlTower.Domain.Entities;

public sealed class Device : Entity
{
    public string? Description { get; private set; }
    public string? Manufacturer { get; private set; }
    public string? Url { get; private set; }
    public bool? IsActive { get; private set; }
    public DateTime? LastCommunication { get; private set; }
    public string? IpAddress { get; private set; }
    public string? Location { get; private set; }
    public string? FirmwareVersion { get; private set; }

    public DateTime? ManufactureDate { get; private set; }
    public string? SerialNumber { get; private set; }
    public ConnectionType ConnectionType { get; private set; }
    public HealthStatus HealthStatus { get; private set; }
    public LastKnownStatus? LastKnownStatus { get; private set; }
    public string? Owner { get; private set; }
    public DateTime? InstallationDate { get; private set; }
    public DateTime? LastMaintenanceDate { get; private set; }
    public SensorType? SensorType { get; private set; }
    public AlarmSettings? AlarmSettings { get; private set; }
    public DateTime? LastHealthCheckDate { get; private set; }
    public MaintenanceHistory? MaintenanceHistory { get; private set; }

    [ForeignKey("User")]
    public string? UserId { get; private set; }
    [JsonIgnore]
    public User? User { get; private set; }

    public Device() { }

    public Device(string description, string manufacturer, string url, bool? isActive, string? userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                  DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                  DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        ValidateAndSetProperties(description, manufacturer, url, isActive.Value, userId, lastCommunication, ipAddress, location, firmwareVersion,
                                 manufactureDate, serialNumber, connectionType, healthStatus, lastKnownStatus, owner, installationDate,
                                 lastMaintenanceDate, sensorType, alarmSettings, lastHealthCheckDate, maintenanceHistory);
    }

    [JsonConstructor]
    public Device(int id, string description, string manufacturer, string url, bool? isActive, string? userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                  DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                  DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        DomainExceptions.When(id < 0, "Invalid Id value");
        Id = id;
        ValidateAndSetProperties(description, manufacturer, url, isActive, userId, lastCommunication, ipAddress, location, firmwareVersion,
                                 manufactureDate, serialNumber, connectionType, healthStatus, lastKnownStatus, owner, installationDate,
                                 lastMaintenanceDate, sensorType, alarmSettings, lastHealthCheckDate, maintenanceHistory);
    }

    public void Update(string description, string manufacturer, string url, bool? isActive, string? userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                       DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                       DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        ValidateAndSetProperties(description, manufacturer, url, isActive, userId, lastCommunication, ipAddress, location, firmwareVersion,
                                 manufactureDate, serialNumber, connectionType, healthStatus, lastKnownStatus, owner, installationDate,
                                 lastMaintenanceDate, sensorType, alarmSettings, lastHealthCheckDate, maintenanceHistory);
    }

    private void ValidateAndSetProperties(string description, string manufacturer, string url, bool? isActive, string? userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                                          DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                                          DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        DomainExceptions.When(string.IsNullOrEmpty(description), "Invalid description. Required");
        DomainExceptions.When(string.IsNullOrEmpty(manufacturer), "Invalid manufacturer. Required");
        DomainExceptions.When(string.IsNullOrEmpty(url), "Invalid url. Required");
        DomainExceptions.When(string.IsNullOrEmpty(userId), "Invalid User. Required");

        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        IsActive = isActive.Value;
        //Who registered and will be responsible for monitoring that device.
        UserId = userId;
        LastCommunication = lastCommunication;
        IpAddress = ipAddress;
        Location = location;
        FirmwareVersion = firmwareVersion;
        ManufactureDate = manufactureDate;
        SerialNumber = serialNumber;
        ConnectionType = connectionType;
        HealthStatus = healthStatus;
        LastKnownStatus = lastKnownStatus;
        Owner = owner;
        InstallationDate = installationDate;
        LastMaintenanceDate = lastMaintenanceDate;
        SensorType = sensorType;
        AlarmSettings = alarmSettings;
        LastHealthCheckDate = lastHealthCheckDate;
        MaintenanceHistory = maintenanceHistory;
    }
}
