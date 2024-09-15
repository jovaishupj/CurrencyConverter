using CurrencyConverter.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CurrencyConverter.Controllers
{
    public class FrankFurterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ICurrencyService _currencyService;

        public FrankFurterController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("api/exchange-rates/{baseCurrency}")]
        public async Task<IActionResult> GetLatestRates(string baseCurrency)
        {
            string error="";
            if (!Validator.isValidCurrency(baseCurrency, out error))
            {
                return BadRequest("Invalid Base Currency");
            }

            try
            {
                var rates = await _currencyService.GetLatestRates(baseCurrency);
                return Ok(rates);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("api/Convert/{fromCurrency}/{toCurrency}/{amount:decimal}")]
        public async Task<IActionResult> Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            string error="";
            if (!Validator.isValidCurrency(fromCurrency, out error) || !Validator.isValidCurrency(toCurrency, out error) ||!Validator.isValidNumber(amount, out error))
            {
                return BadRequest(error);
            }
            if (fromCurrency.ToUpper() == toCurrency.ToUpper())
            {

                return BadRequest("From and To Currency cannot be same");
            }

            try
            {
                var result = await _currencyService.Convert(fromCurrency.ToUpper().Trim(), toCurrency.ToUpper().Trim(), amount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }
        


        [HttpGet("GetHistoricalCurrencyRate")]
        public async Task<IActionResult> GetHistoricalCurrencyRate([FromQuery] string start_date, [FromQuery] string end_date = null,
            [FromQuery] string fromCurrency = null, [FromQuery] string toCurrency = null, [FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 3)
        {
            string error = string.Empty;
            if(!Validator.isValidDate(start_date,out error))
            {
                return BadRequest(error);
            }
            if (!string.IsNullOrEmpty(end_date))
            {
                if (!Validator.isValidDate(end_date, out error))
                {
                    return BadRequest(error);
                }
            }
            if (!string.IsNullOrEmpty(fromCurrency))
            {
                if (!Validator.isValidCurrency(fromCurrency, out error))
                {
                    return BadRequest(error);

                }
            }
            if (!string.IsNullOrEmpty(toCurrency))
            {
                if (!Validator.isValidCurrency(toCurrency, out error))
                {
                    return BadRequest(error);

                }
            }


            try
            {
                var result = await _currencyService.GetHistoricalCurrencyRate(start_date, end_date, fromCurrency, toCurrency, PageNumber, PageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }


    }
}

