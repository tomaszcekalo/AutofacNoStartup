using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AutofacNoStartup
{
    public interface ISalesManagoService
    {
        string GetTypeString();
    }

    public class FakeSalesManagoService : ISalesManagoService
    {
        private SalesManagoSettings _settings;
        private HttpClient _client;
        public JsonSerializerSettings Settings { get; }

        public FakeSalesManagoService(IOptions<SalesManagoSettings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;
            _client = httpClient;
            _client.BaseAddress = new Uri(_settings.Endpoint);
            _client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public FakeSalesManagoService(SalesManagoSettings settings, HttpClient httpClient)
        {
            _settings = settings;
            _client = httpClient;
            _client.BaseAddress = new Uri(_settings.Endpoint);
            _client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public string GetTypeString()
        {
            return nameof(FakeSalesManagoService);
        }
    }
}