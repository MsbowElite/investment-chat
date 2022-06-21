using InvestmentChat.Domain.Constants;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.Services;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using System.Text.Json;

namespace InvestmentChat.Api.Hubs
{
    [SignalRHub]
    public class ChatHub : Hub
    {
        private readonly IRabbitMQPublisher _rabbitMQPublisher;
        private const string StockPrefix = "/stock=";

        public ChatHub(IRabbitMQPublisher rabbitMQPublisher)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task SendMessage(string user, string message, bool isBot = false)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            if (message.StartsWith(StockPrefix) && !isBot)
            {
                var stooqRequest = new StooqRequest() { StockCode = message.Remove(0, StockPrefix.Length) };
                _rabbitMQPublisher.Publish(JsonSerializer.Serialize(stooqRequest), RabbitMQTopics.TopicActionInfo);
            }
        }
    }
}
