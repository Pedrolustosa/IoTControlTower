using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.Device;

public record DeviceDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Url { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastCommunication { get; set; }
    public string? IpAddress { get; set; }
    public string? Location { get; set; }
    public string? FirmwareVersion { get; set; }

    [JsonIgnore]
    public string? Email { get; set; }
}
