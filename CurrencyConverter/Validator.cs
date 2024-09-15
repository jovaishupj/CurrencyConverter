using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CurrencyConverter
{
    public static class Validator
    {
       static  List<string> ValidCurrency = new List<string>{"EUR", "BGN", "AUD", "BRL", "CAD", "CHF", "CNY", "CZK", "DKK", "GBP",
            "HKD", "HUF", "IDR", "ILS", "INR", "ISK", "JPY", "KRW", "MXN", "MYR", "NOK", "NZD", "PHP", "PLN", "RON", "SEK", "SGD", "THB", "TRY", "USD", "ZAR" };

        static List<string> excludedCurrencies = new List<string>() { "TRY", "PLN", "THB", "MXN" };


        public static bool isValidCurrency(string currency, out string error)
        {
            error = string.Empty;
            if(currency == null) 
            {
                error = "Currency Value is null";
                return false;
            }
            if(!ValidCurrency.Contains(currency))
            {
               error = "Invalid Currency";
               return false;
            }
            if(excludedCurrencies.Contains(currency))
            {
                error = "Conversion not supported for TRY, PLN, THB, or MXN currencies.";
                return false;
            }
            return true;
        }
        
        public static bool isValidNumber(decimal Value ,out string error)
        {
            error = string.Empty;

            if (Value <= 0)
            {
                error = "Value Should be greater than 0";
                return false;
            }
            return true;
        }

        public static bool isValidDate(string Date, out string error)
        {
            error = string.Empty;
            if (Date == null)
            {
                return true;
            }

            if (!DateTime.TryParseExact(Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                error = "Invalid date format-Please enter date in yyyy-MM-dd format ";
                return false;
            }
           
            return true;
        }

    }
}
