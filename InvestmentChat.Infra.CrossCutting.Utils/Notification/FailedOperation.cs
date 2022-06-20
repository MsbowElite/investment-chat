using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using System.Text.Json.Serialization;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification
{
    public class FailedOperation : IOperation
    {
        public FailedOperation(ErrorCodes code, MessagesDetail detail)
        {
            Messages = new Messages(code, detail);
        }

        public FailedOperation(ErrorCodes code, List<MessagesDetail> detail)
        {
            Messages = new Messages(code, detail);
        }

        public FailedOperation(ErrorCodes code)
        {
            Messages = new Messages(code);
        }
        public FailedOperation(Messages messages)
        {
            Messages = messages;
        }

        [JsonPropertyName("messages")]
        public Messages Messages { get; }
    }

    public class FailedOperation<T> : FailedOperation, IOperation<T>
    {
        public FailedOperation(ErrorCodes code) : base(code)
        {
        }

        public FailedOperation(Messages messages) : base(messages)
        {
        }

        public FailedOperation(ErrorCodes code, MessagesDetail detail) : base(code, detail)
        {
        }

        public FailedOperation(ErrorCodes code, List<MessagesDetail> detail) : base(code, detail)
        {
        }
    }
}
