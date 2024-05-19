namespace IoTControlTower.Application.DTO
{
    public class CommandDTO
    {
        public string CommandText { get; set; }
        public ICollection<ParameterDTO> Parameters { get; set; }
    }
}
