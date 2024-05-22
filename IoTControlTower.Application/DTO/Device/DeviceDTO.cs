using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO.Device
{
    public class DeviceDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public ICollection<CommandDescriptionDTO> CommandDescriptions { get; set; }
    }
}
