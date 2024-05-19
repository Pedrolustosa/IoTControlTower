namespace IoTControlTower.Domain.Entities
{
    public class Command
    {
        public int Id { get; set; }
        public string CommandText { get; set; }

        public List<Parameter> Parameters { get; set; }
        public List<CommandDescription> CommandDescriptions { get; set; }
    }
}
