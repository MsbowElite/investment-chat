using InvestmentChat.Domain.Services;
using InvestmentChat.Infra.CrossCutting.Utils.Notification;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;

namespace InvestmentChat.Infra.Data.Services
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        public RabbitMQPublisher(RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public IOperation Publish(string message, string topic)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_rabbitMQSettings.Host),
                    Port = _rabbitMQSettings.Port,
                    VirtualHost = _rabbitMQSettings.VirtualHost,
                    UserName = _rabbitMQSettings.UserName,
                    Password = _rabbitMQSettings.Password
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: topic,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicPublish(exchange: "",
                    routingKey: topic,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(message));

                return Result.CreateSuccess();
            }
            catch(Exception ex)
            {
                Log.Logger.Error("{FullName} --- {message} | {ex}", GetType().FullName, message, JsonSerializer.Serialize(ex));
                return Result.CreateFailure(ErrorCodes.FailedSendRabbitMQMessage);
            }
        }
    }
}
