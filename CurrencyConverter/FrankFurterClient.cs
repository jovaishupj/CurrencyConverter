using System.Text.Json;
using System.Net.Http;

namespace CurrencyConverter
{
    public class FrankFurterClient
    {

        private readonly string _baseUrl = "https://api.frankfurter.app/";

        public async Task<Dictionary<string, decimal>> GetLatestRates(string baseCurrency)
        {
            using var httpClient = new HttpClient();

            var url = $"{_baseUrl}+latest ? base={baseCurrency}";

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<Dictionary<string, decimal>>(contentString);
                return rates;
            }
            else
            {
                throw new Exception($"Error retrieving exchange rates: {response.StatusCode}");
            }
        }
    }
}
