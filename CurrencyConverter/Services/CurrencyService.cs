
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace CurrencyConverter.Services
{
    public class CurrencyService : ICurrencyService
    {

        private readonly string _baseUrl = "https://api.frankfurter.app/";
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CurrencyService> _logger;
        public CurrencyService(HttpClient httpClient, IMemoryCache cache, ILogger<CurrencyService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }


        public async Task<FrankFurter> GetLatestRates(string baseCurrency)
        {

            string cacheKey = $"ExchangeRates_{baseCurrency}";
            if (_cache.TryGetValue(cacheKey, out FrankFurter cachedRates))
            {
                return cachedRates;
            }

            var url = $"{_baseUrl}latest?from={baseCurrency}";
            var latestrates = await RetryHelper.ExecuteWithRetryAsync<FrankFurter>(
           async () =>
           {
               var response = await _httpClient.GetAsync(url);
               response.EnsureSuccessStatusCode();
               var contentString = await response.Content.ReadAsStringAsync();
               FrankFurter exchangeRates = JsonSerializer.Deserialize<FrankFurter>(contentString);

               return exchangeRates;
           },
           maxRetries: 3, initialDelayMilliseconds: 1000, logger: _logger);

            _cache.Set(cacheKey, latestrates, TimeSpan.FromMinutes(10));
            return latestrates;
        }


        public async Task<FrankFurter> Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            string cacheKey = $"ExchangeRates_{fromCurrency}_{toCurrency}_{amount}";
            if (_cache.TryGetValue(cacheKey, out FrankFurter cachedRates))
            {
                return cachedRates;
            }

            var url = $"{_baseUrl}latest?amount={amount}&from={fromCurrency}&to={toCurrency}";


            var ConverterdRate = await RetryHelper.ExecuteWithRetryAsync<FrankFurter>(
          async () =>
          {
              var response = await _httpClient.GetAsync(url);
              response.EnsureSuccessStatusCode();
              var contentString = await response.Content.ReadAsStringAsync();
              FrankFurter converted = JsonSerializer.Deserialize<FrankFurter>(contentString);

              return converted;
          },
         maxRetries: 3, initialDelayMilliseconds: 1000, logger: _logger);

            _cache.Set(cacheKey, ConverterdRate, TimeSpan.FromMinutes(10));
            return ConverterdRate;
        }

        public async Task<HistoricalCurrencyData> GetHistoricalCurrencyRate(string start_date, string end_date = null,
            string? fromCurrency = null, string toCurrency = null, int PageNumber = 1, int PageSize = 3)
        {

            var skipNumber = (PageNumber - 1) * PageSize;
            string cacheKey = $"ExchangeRates_{start_date}_{end_date}_{fromCurrency}_{toCurrency}";
            if (_cache.TryGetValue(cacheKey, out HistoricalCurrencyRate cachedRates))
            {

                HistoricalCurrencyData cachedData = new HistoricalCurrencyData();
                cachedData.TotalRecords = cachedRates.rates.Count;
                cachedData.PagedRate = cachedRates.rates.Skip(skipNumber).Take(PageSize).ToDictionary();
                return cachedData;

            }

            var url = $"{_baseUrl}{start_date}..";
            url = !string.IsNullOrEmpty(end_date) ? url + end_date : url;
            url = !string.IsNullOrEmpty(fromCurrency) ? url + "?from=" + fromCurrency : url;
            if (url.Contains("?"))
            {
                url = !string.IsNullOrEmpty(toCurrency) ? url + "&to=" + toCurrency : url;
            }
            else
            {
                url = !string.IsNullOrEmpty(toCurrency) ? url + "?to=" + toCurrency : url;
            }


            var ConverterdRate = await RetryHelper.ExecuteWithRetryAsync<HistoricalCurrencyRate>(
        async () =>
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var contentString = await response.Content.ReadAsStringAsync();
            HistoricalCurrencyRate conversionRate = JsonSerializer.Deserialize<HistoricalCurrencyRate>(contentString);


            return conversionRate;
        },
        maxRetries: 3, initialDelayMilliseconds: 1000, logger: _logger);

            _cache.Set(cacheKey, ConverterdRate, TimeSpan.FromMinutes(10));



            HistoricalCurrencyData currencyData = new HistoricalCurrencyData();
            currencyData.TotalRecords = ConverterdRate.rates.Count;
            currencyData.PagedRate = ConverterdRate.rates.Skip(skipNumber).Take(PageSize).ToDictionary();
            return currencyData;


        }

    }
}
