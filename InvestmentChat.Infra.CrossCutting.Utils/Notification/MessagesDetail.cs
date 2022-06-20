using System.Text.Json.Serialization;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification
{
    public class MessagesDetail
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
