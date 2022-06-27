using System.ComponentModel;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums
{
    public enum ErrorCodes
    {
        [Description("Default error.")]
        DefaultError = 1,
        [Description("Command default error.")]
        CommandDefaultError = 401,
        [Description("Failed to send message to RabbitMQ.")]
        FailedSendRabbitMQMessage = 501,
        [Description("Failed to execute webrequest.")]
        FailedHttpClient = 502,
    }
}
