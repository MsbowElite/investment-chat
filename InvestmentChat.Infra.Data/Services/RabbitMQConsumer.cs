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
    public abstract class RabbitMQConsumer : BackgroundService
    {
        protected IConnection _connection;
        protected IModel _channel;

        public RabbitMQConsumer(RabbitMQSettings rabbitMQSettings, string topic)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMQSettings.Host),
                Port = rabbitMQSettings.Port,
                VirtualHost = rabbitMQSettings.VirtualHost,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password,
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: topic,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }
}
