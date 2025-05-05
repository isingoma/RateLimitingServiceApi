using Microsoft.Extensions.Options;
using RateLimiterApi.Models;

namespace RateLimiterApi.Services
{
    public interface IRateLimitService
    {
        bool AllowRequest(string clientId); 
    }

    public class InMemoryRateLimitService : IRateLimitService
    {
        private readonly Dictionary<string, Queue<DateTime>> _requestLogs = new();
        private readonly int _requestsAllowed;
        private readonly TimeSpan _timeWindow;
        private readonly object _lock = new();

        public InMemoryRateLimitService(IOptions<RateLimitOptions> options)
        {
            _requestsAllowed = options.Value.RequestsAllowed;
            _timeWindow = TimeSpan.FromSeconds(options.Value.TimeWindowInSeconds);
        }

        public bool AllowRequest(string key)
        {
            lock (_lock)
            {
                // Initialize log for new keys
                if (!_requestLogs.TryGetValue(key, out var timestamps))
                {
                    timestamps = new Queue<DateTime>();
                    _requestLogs[key] = timestamps;
                }

                var now = DateTime.UtcNow;

                // Remove timestamps outside of the time window
                while (timestamps.Count > 0 && (now - timestamps.Peek()) > _timeWindow)
                    timestamps.Dequeue();

                // Deny request if the limit is exceeded
                if (timestamps.Count >= _requestsAllowed)
                    return false;

                // Allow request and log timestamp
                timestamps.Enqueue(now);
                return true;
            }
        }
    }

}
