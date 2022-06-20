using Newtonsoft.Json;

namespace InvestmentChat.Infra.Data.HttpClients
{
    public abstract class BaseHttpClient : HttpClient
    {
        private static readonly JsonSerializer _serializer
            = JsonSerializer.Create();

        protected readonly HttpClient _httpClient;

        protected BaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async ValueTask<T> ReadJsonResponse<T>(HttpResponseMessage response)
        {
            using (Stream s = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return _serializer.Deserialize<T>(reader);
            }
        }
    }
}
