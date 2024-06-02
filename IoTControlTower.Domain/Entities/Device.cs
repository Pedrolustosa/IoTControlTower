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

    [ForeignKey("User")]
    public string UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }

    public Device() { }

    public Device(string description, string manufacturer, string url, bool? isActive, string userId)
    {
        ValidateDomain(description, manufacturer, url, isActive.Value, userId);
    }

    [JsonConstructor]
    public Device(int id, string description, string manufacturer, string url, bool? isActive, string userId)
    {
        DomainValidation.When(id < 0, "Invalid Id value");
        Id = id;
        ValidateDomain(description, manufacturer, url, isActive, userId);
    }

    public void Update(string description, string manufacturer, string url, bool? isActive, string userId)
    {
        ValidateDomain(description, manufacturer, url, isActive, userId);
    }

    private void ValidateDomain(string description, string manufacturer, string url, bool? isActive, string userId)
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
    }
}
