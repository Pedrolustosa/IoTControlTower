using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.Interfaces;
using IoTControlTower.Domain.Interfaces.RabbitRepository;

namespace IoTControlTower.Application.Services;

public class RabbitMQService(IRabbitMQRepository rabbitMQRepository) : IRabbitMQService
{
    private readonly IRabbitMQRepository _repository = rabbitMQRepository;

    public void ConsumeMessages(Action<RabbitMessage> onMessageReceived) => _repository.ConsumeMessages(onMessageReceived);

    public void SendMessage(RabbitMessage rabbitMessage) => _repository.SendMessage(rabbitMessage);
}
