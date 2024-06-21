using System.Text.Json;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Application.Interfaces;
using IoTControlTower.Application.CQRS.Devices.Commands;

namespace IoTControlTower.Application.Services;

public class RabbitMQConsumerService(IRabbitMQService rabbitMQService, ILogger<RabbitMQConsumerService> logger) : IRabbitMQConsumerService
{
    private readonly IRabbitMQService _rabbitMQService = rabbitMQService;
    private readonly ILogger<RabbitMQConsumerService> _logger = logger;

    public void StartConsuming()
    {
        _rabbitMQService.ConsumeMessages(OnMessageReceived);
    }

    private void OnMessageReceived(RabbitMessage message)
    {
        _logger.LogInformation("Message received: {Title} - {Text}", message.Title, message.Text);
        var createDeviceCommand = JsonSerializer.Deserialize<CreateDeviceCommand>(message.Text);
    }
}
