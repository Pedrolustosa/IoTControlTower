namespace IoTControlTower.Application.DTO.Device;

public record DeviceDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Manufacturer { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; }
}
