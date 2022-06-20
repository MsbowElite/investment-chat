using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentChat.Domain.Services
{
    public interface IRabbitMQPublisher
    {
        IOperation Publish(string message, string topic);
    }
}
