using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;

namespace InvestmentChat.Domain.Services
{
    public interface IRabbitMQPublisher
    {
        IOperation Publish(string message, string topic);
    }
}
