using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;

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

    [ForeignKey("User")]
    public string UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }

    public Device() { }

    public Device(string description, string manufacturer, string url, bool? isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion)
    {
        ValidateDomain(description, manufacturer, url, isActive.Value, userId, lastCommunication, ipAddress, location, firmwareVersion);
    }

    [JsonConstructor]
    public Device(int id, string description, string manufacturer, string url, bool? isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion)
    {
        DomainValidation.When(id < 0, "Invalid Id value");
        Id = id;
        ValidateDomain(description, manufacturer, url, isActive, userId, lastCommunication, ipAddress, location, firmwareVersion);
    }

    public void Update(string description, string manufacturer, string url, bool? isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion)
    {
        ValidateDomain(description, manufacturer, url, isActive, userId, lastCommunication, ipAddress, location, firmwareVersion);
    }

    private void ValidateDomain(string description, string manufacturer, string url, bool? isActive, string userId, DateTime? lastCommunication, string? ipAddress, string? location, string? firmwareVersion)
    {
        DomainValidation.When(string.IsNullOrEmpty(description), "Invalid description. Required");
        DomainValidation.When(string.IsNullOrEmpty(manufacturer), "Invalid manufacturer. Required");
        DomainValidation.When(string.IsNullOrEmpty(url), "Invalid url. Required");
        DomainValidation.When(string.IsNullOrEmpty(userId), "Invalid User. Required");

        Description = description;
        Manufacturer = manufacturer;
        Url = url;
        IsActive = isActive.Value;
        UserId = userId;
        LastCommunication = lastCommunication;
        IpAddress = ipAddress;
        Location = location;
        FirmwareVersion = firmwareVersion;
    }
}
