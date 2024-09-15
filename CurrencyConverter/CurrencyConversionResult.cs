using System.Net;

namespace CurrencyConverter
{
    public class CurrencyConversionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public decimal ConvertedAmount { get; set; }

        public CurrencyConversionResult(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public CurrencyConversionResult(HttpStatusCode statusCode, decimal convertedAmount)
        {
            StatusCode = statusCode;
            ConvertedAmount = convertedAmount;
        }
    }
}
