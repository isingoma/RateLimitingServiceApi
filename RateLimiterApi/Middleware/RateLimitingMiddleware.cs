using RateLimiterApi.Services;

namespace RateLimiterApi.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRateLimitService _rateLimitService;

        public RateLimitingMiddleware(RequestDelegate next, IRateLimitService rateLimitService)
        {
            _next = next;
            _rateLimitService = rateLimitService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Check if the request exceeds rate limit
            if (!_rateLimitService.AllowRequest(ipAddress))
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            // Proceed with the next middleware in the pipeline
            await _next(context);
        }
    }
}
