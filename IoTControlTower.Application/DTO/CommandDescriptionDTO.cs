using System.Text.Json.Serialization;

namespace IoTControlTower.Application.DTO
{
    public class CommandDescriptionDTO
    {
        public string Operation { get; set; }
        public string Description { get; set; }
        public string Result { get; set; }
        public string Format { get; set; }
        public int DeviceId { get; set; }
        public string DeviceIdentifier { get; set; }
        public int CommandId { get; set; }
        public CommandDTO Command { get; set; }
    }
}
