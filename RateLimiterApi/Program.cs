using RateLimiterApi.Middleware;
using RateLimiterApi.Models;
using RateLimiterApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register RateLimit config
builder.Services.Configure<RateLimitOptions>(
    builder.Configuration.GetSection("RateLimiting"));

// Register services
builder.Services.AddSingleton<IRateLimitService, InMemoryRateLimitService>();

// ✅ Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// ✅ Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();
app.Run();
