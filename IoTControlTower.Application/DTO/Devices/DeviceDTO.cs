using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.Device;

public record DeviceDTO
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
    public string? Url { get; set; }
    public bool IsActive { get; set; }

    [JsonIgnore]
    public string? Email { get; set; }
}
