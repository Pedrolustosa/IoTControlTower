namespace IoTControlTower.Domain.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<CommandDescription> CommandDescriptions { get; set; }
    }
}
