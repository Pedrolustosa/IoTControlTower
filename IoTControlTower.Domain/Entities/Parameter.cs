namespace IoTControlTower.Domain.Entities
{
    public class Parameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CommandId { get; set; }
        public Command Command { get; set; }
    }
}
