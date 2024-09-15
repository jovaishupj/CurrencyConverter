namespace CurrencyConverter.Services
{
    public interface ICurrencyService
    {
        Task<FrankFurter> GetLatestRates(string baseCurrency);
        Task<FrankFurter> Convert(string fromCurrency, string toCurrency, decimal amount);

        Task<HistoricalCurrencyData> GetHistoricalCurrencyRate(string start_date, string end_date=null,
            string fromCurrency=null, string toCurrency=null, int PageNumber = 1, int PageSize = 3);


    }
}
