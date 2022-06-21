using InvestmentChat.Domain.Dto;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;

namespace InvestmentChat.Domain.HttpClients
{
    public interface IStooqClient
    {
        Task<IOperation> GetStooq(StooqRequest stooqRequest);
    }
}
