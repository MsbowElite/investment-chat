using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Extensions;

namespace InvestmentChat.Infra.CrossCutting.Utils.Notification
{
    public class Messages
    {
        public Messages(ErrorCodes code, MessagesDetail field)
        {
            Code = code;
            AddField(field);
        }

        public Messages(ErrorCodes code, List<MessagesDetail> field)
        {
            Code = code;
            Fields = field;
        }

        public Messages(ErrorCodes code)
        {
            Code = code;
        }

        [JsonPropertyName("code")]
        public ErrorCodes Code { get; }
        [JsonPropertyName("message")]
        public string Message { get => Code.GetAttributeOfType<DescriptionAttribute>().Description; }
        [JsonPropertyName("fields")]
        public List<MessagesDetail> Fields { get; private set; } = new List<MessagesDetail>();

        public void AddField(MessagesDetail detail)
        {
            if (detail is object)
                Fields.Add(detail);
        }
    }
}
