using InvestmentChat.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using SignalRSwaggerGen.Attributes;
using System.Text.Json;

namespace InvestmentChat.Api.Hubs
{
    //[Authorize(Roles = SD.Admin)]
    [SignalRHub]
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error("{FullName} --- User:{user} | Message:{message} Exception:{ex}",
                    GetType().FullName,
                    user,
                    message,
                    JsonSerializer.Serialize(ex));
            }

            await _mediator.Send(new AnalyzerStockMessageCommand(message));
        }
    }
}
