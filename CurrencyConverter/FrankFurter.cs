using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyConverter
{
    public class FrankFurter
    {
      
        public double amount { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }
        public DateTime date { get; set; }
        public Dictionary<string, double> rates { get; set; }
    }

    public class HistoricalCurrencyRate : FrankFurter
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<DateTime, Dictionary<string, double>> rates { get; set; }
       

    }

    public class HistoricalCurrencyData
    {
        public int TotalRecords {  get; set; }
        public Dictionary<DateTime, Dictionary<string, double>> PagedRate { get; set; }
    }
}
