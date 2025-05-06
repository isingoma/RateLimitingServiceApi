# Rate Limiting Service Api (.NET 8 Web API)

## 📌 Overview

This project demonstrates a custom **Rate Limiting Service Api** built using .NET 8 Web API. The middleware is designed to prevent abuse of API endpoints by limiting the number of requests from a given IP address within a configurable time window.

It is highly relevant in scenarios where:
- You want to protect your backend from brute-force or DoS attacks.
- You need per-user/IP throttling.
- You want a lightweight and configurable solution without relying on external API gateways.

---

### 📌 Problem, Context, and Solution
❓ **Problem Being Solved**

APIs exposed to the public or third-party clients are vulnerable to abuse through excessive or malicious requests (e.g., brute-force login attempts, scraping, DoS attacks). Without proper control, this leads to:
- Degraded performance or downtime
- Increased infrastructure costs
- Poor user experience

### 🧩 Complexity Involved
- Tracking and limiting user/IP requests in real time
- Ensuring thread safety and performance under concurrent traffic
- Keeping the solution lightweight and configurable
- Optionally enabling it to scale across servers or containers

### ✅ Solution Implemented
- I developed a custom rate-limiting middleware using .NET 8 that:
- Intercepts all incoming requests
- Tracks requests per IP in memory
- Throttles requests based on limits defined in appsettings.json
- Returns HTTP 429 when limits are exceeded

---

## 🚀 How It Works

- Middleware checks every incoming request's IP address.
- Each IP has an in-memory queue of timestamps (representing past requests).
- If the number of requests in the defined time window exceeds the limit, the middleware returns **HTTP 429 Too Many Requests**.
- You can customize the rate limit via `appsettings.json`.

---

### **📁 Project Structure**

| File                          | Purpose                                              |
| ----------------------------- | ---------------------------------------------------- |
| `RateLimitingMiddleware.cs`   | Core logic for intercepting and throttling requests  |
| `InMemoryRateLimitService.cs` | Tracks request timestamps per IP                     |
| `RateLimitOptions.cs`         | Holds configuration from `appsettings.json`          |
| `TestController.cs`           | Simple API to test rate limiting                     |
| `RateLimitingTests.cs`        | Unit tests to verify middleware and service behavior |
| `Program.cs`                  | middleware, Swagger, and controllers                 |

---

### **🧪 Quality Checks (Unit Tests)**
Written using xUnit which covers; 
- Whether limits are respected correctly.
- Middleware behavior when over-limit is reached.

---

### **⚙️ How to Run Locally**

**1. Clone the repository:**
git clone https://github.com/isingoma/RateLimitingServiceApi.git


**3. Try it in Swagger UI:**
Visit: https://localhost:5001/swagger
Use: GET /api/ping

**4. Test the rate limit:**
Call /api/ping more than 5 times in 10 seconds → You'll receive:

{
  "status": 429,
  "message": "Too many requests. Please try again later."
}

---

### **💡 Why This Matters**
This feature demonstrates:
- Middleware usage
- DI and configuration best practices
- Security-conscious API design
- Scalable logic (easily swappable to Redis for distributed caching)

---

### **🔄 Future Improvements**
- Store request logs in Redis for load-balanced or multi-instance APIs
- Add authentication-based throttling (e.g., by UserId)
- Include rate limit headers in response

---
