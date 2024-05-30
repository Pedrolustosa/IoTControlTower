using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;

namespace IoTControlTower.Domain.Entities;

public class Device : Entity
{
    public string? Description { get; private set; }
    public string? Manufacturer { get; private set; }
    public string? Url { get; private set; }
    public bool? IsActive { get; private set; }

    public Device() { }

    public Device(string description, string manufacturer, string url, bool? isActive)
    {
        ValidateDomain(description, manufacturer, url, isActive);
    }

    [JsonConstructor]
    public Device(int id, string description, string manufacturer, string url, bool? isActive)
    {
        DomainValidation.When(id < 0, "Invalid Id value");
        Id = id;
        ValidateDomain(description, manufacturer, url, isActive);
    }

    public void Update(string description, string manufacturer, string url, bool? isActive)
    {
        ValidateDomain(description, manufacturer, url, isActive);
    }

    private void ValidateDomain(string description, string manufacturer, string url, bool? isActive)
    {
        DomainValidation.When(string.IsNullOrEmpty(description), "Invalid description. Required");
        DomainValidation.When(string.IsNullOrEmpty(manufacturer), "Invalid manufacturer. Required");
        DomainValidation.When(string.IsNullOrEmpty(url), "Invalid url. Required");

        Description = description;
        Manufacturer = manufacturer; 
        Url = url;
        IsActive = isActive.Value;
    }
}
