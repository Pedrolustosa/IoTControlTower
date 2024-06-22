using IoTControlTower.Domain.Enums;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControlTower.Domain.Entities;

public sealed class Device : Entity
{
    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; private set; }

    [Required(ErrorMessage = "Manufacturer is required")]
    public string? Manufacturer { get; private set; }

    [Required(ErrorMessage = "Url is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    public string? Url { get; private set; }

    public bool IsActive { get; private set; }
    public DateTime? LastCommunication { get; private set; }

    [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", ErrorMessage = "Invalid IP Address")]
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
    [Required(ErrorMessage = "UserId is required")]
    public string? UserId { get; private set; }

    [JsonIgnore]
    public User? User { get; private set; }

    private Device() { }

    public Device(string description, string manufacturer, string url, bool isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                  DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                  DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        Validate(description, manufacturer, url, userId);

        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        IsActive = isActive;
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

    public void Update(string description, string manufacturer, string url, bool isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion,
                       DateTime? manufactureDate, string? serialNumber, ConnectionType connectionType, HealthStatus healthStatus, LastKnownStatus? lastKnownStatus, string? owner, DateTime? installationDate,
                       DateTime? lastMaintenanceDate, SensorType? sensorType, AlarmSettings? alarmSettings, DateTime? lastHealthCheckDate, MaintenanceHistory? maintenanceHistory)
    {
        Validate(description, manufacturer, url, userId);

        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        IsActive = isActive;
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

    private static void Validate(string description, string manufacturer, string url, string userId)
    {
        DomainExceptions.When(string.IsNullOrEmpty(description), "Invalid description. Required");
        DomainExceptions.When(string.IsNullOrEmpty(manufacturer), "Invalid manufacturer. Required");
        DomainExceptions.When(string.IsNullOrEmpty(url), "Invalid url. Required");
        DomainExceptions.When(string.IsNullOrEmpty(userId), "Invalid User. Required");
    }
}