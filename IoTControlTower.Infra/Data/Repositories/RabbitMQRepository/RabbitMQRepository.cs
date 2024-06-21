using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interfaces.RabbitRepository;

namespace IoTControlTower.Infra.Data.Repositories.RabbitMQRepository;

public class RabbitMQRepository : IRabbitMQRepository
{
    public void ConsumeMessages(Action<RabbitMessage> onMessageReceived)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "CreateDeviceQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var rabbitMessage = JsonSerializer.Deserialize<RabbitMessage>(message);
            onMessageReceived(rabbitMessage);
        };

        channel.BasicConsume(queue: "CreateDeviceQueue",
                             autoAck: true,
                             consumer: consumer);
    }

    public void SendMessage(RabbitMessage rabbitMessage)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "RabbitMessageQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        string json = JsonSerializer.Serialize(rabbitMessage);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "",
                             routingKey: "RabbitMessageQueue",
                             basicProperties: null,
                             body: body);
    }
}
