namespace IoTControlTower.Domain.Entities;

public class RabbitMessage
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public string? Payload { get; set; }
}
