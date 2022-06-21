using InvestmentChat.Api.Hubs;
using InvestmentChat.Domain.Constants;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.HttpClients;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using InvestmentChat.Infra.Data.Services;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InvestmentChat.Api.Consumers
{
    public class BotMessageConsumer : RabbitMQConsumer
    {
        private const string BotUser = "Bot";
        private readonly IHubContext<ChatHub> _hubContext;

        public BotMessageConsumer(IHubContext<ChatHub> hubContext, RabbitMQSettings rabbitMQSettings)
            : base(rabbitMQSettings, RabbitMQTopics.TopicBotMessage)
        {
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<StooqInfo>(content);

                if (message is not null)
                {
                    _hubContext.Clients.All.SendAsync("ReceiveMessage", BotUser, JsonSerializer.Serialize(message)).Wait();
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQTopics.TopicBotMessage, false, consumer);

            return Task.CompletedTask;
        }
    }
}