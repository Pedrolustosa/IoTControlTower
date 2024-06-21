using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Domain.Interfaces.RabbitRepository;

public interface IRabbitMQRepository
{
    void SendMessage(RabbitMessage rabbitMessage);
    void ConsumeMessages(Action<RabbitMessage> onMessageReceived);
}
