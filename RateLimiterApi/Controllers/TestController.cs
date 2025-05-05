using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RateLimiterApi.Controllers
{
    // Basic test controller to validate rate limiting
    [ApiController]
    [Route("api")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Requests within the Time Range.");
        }
    }
}
