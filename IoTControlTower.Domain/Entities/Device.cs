namespace IoTControlTower.Domain.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }

        public ICollection<CommandDescription> CommandDescriptions { get; set; }
    }
}
