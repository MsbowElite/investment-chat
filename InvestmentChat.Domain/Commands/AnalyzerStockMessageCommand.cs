using InvestmentChat.Domain.Constants;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.Services;
using InvestmentChat.Infra.CrossCutting.Utils.Notification;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvestmentChat.Domain.Commands
{
    public class AnalyzerStockMessageCommand : IRequest<IOperation>
    {
        public AnalyzerStockMessageCommand(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class AnalyzerStockMessageCommandHandler : IRequestHandler<AnalyzerStockMessageCommand, IOperation>
    {
        private const string _stockPrefix = "/stock=";
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public AnalyzerStockMessageCommandHandler(IRabbitMQPublisher rabbitMQPublisher)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<IOperation> Handle(AnalyzerStockMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Message.StartsWith(_stockPrefix))
                {
                    var stooqRequest = new StooqRequest() { StockCode = request.Message.Remove(0, _stockPrefix.Length) };
                    return _rabbitMQPublisher.Publish(JsonSerializer.Serialize(stooqRequest), RabbitMQTopics.TopicActionInfo);
                }

                return Result.CreateSuccess();
            }
            catch (Exception ex)
            {
                Log.Logger.Error("{FullName} --- Dto:{request} | Exception:{ex}", 
                    GetType().FullName, 
                    JsonSerializer.Serialize(request),
                    JsonSerializer.Serialize(ex));

                return Result.CreateFailure(ErrorCodes.CommandDefaultError);
            }
        }
    }
}
