using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Application.Interfaces;

public interface IRabbitMQService
{
    void SendMessage(RabbitMessage rabbitMessage);
    void ConsumeMessages(Action<RabbitMessage> onMessageReceived);
}
