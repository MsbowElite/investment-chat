using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using System.Text.Json.Serialization;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification
{
    public class InvalidOperation<T> : IOperation<T>
    {
        public InvalidOperation(ErrorCodes code, MessagesDetail detail)
        {
            Messages = new Messages(code, detail);
        }

        public InvalidOperation(ErrorCodes code, List<MessagesDetail> detail)
        {
            Messages = new Messages(code, detail);
        }

        public InvalidOperation(ErrorCodes code)
        {
            Messages = new Messages(code);
        }

        [JsonPropertyName("messages")]
        public Messages Messages { get; }
    }
}
