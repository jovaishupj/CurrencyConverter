namespace CurrencyConverter
{
    public class RetryHelper
    {
        public static async Task<T> ExecuteWithRetryAsync<T>(
        Func<Task<T>> action,
        int maxRetries = 3,
        int initialDelayMilliseconds = 1000,
        ILogger logger = null)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (HttpRequestException ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw; // Re-throw if maximum retries are reached
                    }

                    var delay = TimeSpan.FromMilliseconds(initialDelayMilliseconds * Math.Pow(2, retryCount - 1));
                    logger?.LogWarning(ex, $"Request failed. Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay);
                }
            }
        }
    }
}
