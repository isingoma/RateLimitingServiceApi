namespace RateLimiterApi.Models
{
    public class RateLimitOptions
    {
        public int RequestsAllowed { get; set; }
        public int TimeWindowInSeconds { get; set; }    
    }
}
