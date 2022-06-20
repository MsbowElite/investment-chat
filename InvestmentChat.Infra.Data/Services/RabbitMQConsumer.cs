using InvestmentChat.Domain.Constants;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.HttpClients;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InvestmentChat.Infra.Data.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IStooqClient _stooqClient;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(IStooqClient stooqClient, RabbitMQSettings rabbitMQSettings)
        {
            _stooqClient = stooqClient;

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQSettings.Host),
                Port = rabbitMQSettings.Port,
                VirtualHost = rabbitMQSettings.VirtualHost,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: RabbitMQTopics.TopicActionInfo,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<StooqRequest>(content);

                if (message is not null)
                    _stooqClient.GetStooqCsv(message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQTopics.TopicActionInfo, false, consumer);

            return Task.CompletedTask;
        }
    }
}
