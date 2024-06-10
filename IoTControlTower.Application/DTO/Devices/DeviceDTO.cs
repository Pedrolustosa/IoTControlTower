using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.Device;

public record DeviceDTO(int Id, string? Description, string? Manufacturer, string? Url, bool IsActive,
                        DateTime? LastCommunication, string? IpAddress, string? Location, string? FirmwareVersion)
{
    [JsonIgnore]
    public string? Email { get; set; }
};

