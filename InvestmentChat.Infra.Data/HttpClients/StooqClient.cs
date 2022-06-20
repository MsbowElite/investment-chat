using ChoETL;
using InvestmentChat.Domain.Dto;
using InvestmentChat.Domain.HttpClients;
using InvestmentChat.Infra.CrossCutting.Utils.Notification;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Enums;
using InvestmentChat.Infra.CrossCutting.Utils.Notification.Interfaces;
using System.Text;
using System.Text.Json;

namespace InvestmentChat.Infra.Data.HttpClients
{
    public class StooqClient : BaseHttpClient, IStooqClient
    {
        private const string _requestComplement = "&f=sd2t2ohlcv&h&e=csv";

        public StooqClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<IOperation> GetStooqCsv(StooqRequest stooqRequest)
        {
            try
            {
                var response = await _httpClient.GetByteArrayAsync($"?s={stooqRequest.StockCode}{_requestComplement}");

                var stringContent = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);

                StringBuilder sb = new StringBuilder();
                using (var p = ChoCSVReader.LoadText(stringContent)
                    .WithFirstLineHeader()
                    )
                {
                    using (var w = new ChoJSONWriter(sb))
                        w.Write(p);
                }
                var json = JsonSerializer.Deserialize<List<StooqInfo>>(sb.ToString());
                return Result.CreateSuccess(json);

            }
            catch (Exception ex)
            {

            }

            return Result.CreateFailure(ErrorCodes.FailedHttpClient);
        }
    }
}
