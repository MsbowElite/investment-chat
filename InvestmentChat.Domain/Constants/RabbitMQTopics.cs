using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentChat.Domain.Constants
{
    public static class RabbitMQTopics
    {
        public const string TopicActionInfo = "ActionInfo";
        public const string TopicBotMessage = "BotMessage";
    }
}
