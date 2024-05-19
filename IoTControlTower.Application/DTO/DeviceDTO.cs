using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO
{
    public class DeviceDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public ICollection<CommandDescriptionDTO> CommandDescriptions { get; set; }
    }
}
