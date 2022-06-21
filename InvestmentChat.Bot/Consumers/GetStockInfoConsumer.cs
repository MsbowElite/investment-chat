using InvestmentChat.Domain.Constants;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.HttpClients;
using InvestmentChat.Domain.Services;
using InvestmentChat.Infra.CrossCutting.Utils.Notification;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using InvestmentChat.Infra.Data.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InvestmentChat.Bot.Consumers
{
    public class GetStockInfoConsumer : RabbitMQConsumer
    {
        private readonly IStooqClient _stooqClient;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public GetStockInfoConsumer(IStooqClient stooqClient, IRabbitMQPublisher rabbitMQPublisher, RabbitMQSettings rabbitMQSettings)
            : base(rabbitMQSettings, RabbitMQTopics.TopicActionInfo)
        {
            _stooqClient = stooqClient;
            _rabbitMQPublisher = rabbitMQPublisher;
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
                {
                    if (_stooqClient.GetStooq(message).Result is SuccessfulOperation<StooqInfo> getStooqResult)
                    {
                        _rabbitMQPublisher.Publish(JsonSerializer.Serialize(getStooqResult.Data),
                            RabbitMQTopics.TopicBotMessage);
                    }
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQTopics.TopicActionInfo, false, consumer);

            return Task.CompletedTask;
        }
    }
}