using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using System.Text.Json.Serialization;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification
{
    public class SuccessfulOperation : IOperation
    {

    }

    public class SuccessfulOperation<T> : SuccessfulOperation, IOperation<T>
    {
        public SuccessfulOperation(T data)
        {
            Data = data;
        }

        [JsonPropertyName("data")]
        public T Data { get; }

        public static implicit operator SuccessfulOperation<T>(T data)
        {
            return new SuccessfulOperation<T>(data);
        }
    }
}
