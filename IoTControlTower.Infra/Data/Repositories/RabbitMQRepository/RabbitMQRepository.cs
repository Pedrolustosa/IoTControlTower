using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using IoTControlTower.Domain.Entities;
using Microsoft.Extensions.Configuration;
using IoTControlTower.Domain.Interfaces.RabbitRepository;

namespace IoTControlTower.Infra.Data.Repositories.RabbitMQRepository
{
    public class RabbitMQRepository(IConfiguration configuration) : IRabbitMQRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public void ConsumeMessages(Action<RabbitMessage> onMessageReceived)
        {
            ConnectionFactory factory = ConnectionFactory();
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
            ConnectionFactory factory = ConnectionFactory();
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

        private ConnectionFactory ConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };
        }
    }
}
