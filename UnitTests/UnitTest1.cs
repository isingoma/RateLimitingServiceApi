using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RateLimiterApi.Middleware;
using RateLimiterApi.Models;
using RateLimiterApi.Services;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void AllowRequest_LimitsExceeded_ReturnsFalse()
        {
            // Set up a rate limit of 2 requests per second
            var options = Options.Create(new RateLimitOptions { RequestsAllowed = 2, TimeWindowInSeconds = 1 });
            var service = new InMemoryRateLimitService(options);

            var result1 = service.AllowRequest("test");
            var result2 = service.AllowRequest("test");
            var result3 = service.AllowRequest("test");

            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
        }

        [Fact]
        public async Task Middleware_BlocksRequests_WhenLimitExceeded()
        {
            //Only one request allowed within 10 seconds.
            var options = Options.Create(new RateLimitOptions { RequestsAllowed = 1, TimeWindowInSeconds = 10 });
            var service = new InMemoryRateLimitService(options);

            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            var middleware = new RateLimitingMiddleware(_ => Task.CompletedTask, service);

            // First request should pass
            await middleware.InvokeAsync(context);

            // Second request should be blocked
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;
            await middleware.InvokeAsync(context);

            Assert.Equal(429, context.Response.StatusCode);
        }
    }
}