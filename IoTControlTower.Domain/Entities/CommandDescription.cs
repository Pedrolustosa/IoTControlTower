namespace IoTControlTower.Domain.Entities
{
    public class CommandDescription
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public string Result { get; set; }
        public string Format { get; set; }
        public string DeviceIdentifier { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int CommandId { get; set; }
        public Command Command { get; set; }
    }
}
