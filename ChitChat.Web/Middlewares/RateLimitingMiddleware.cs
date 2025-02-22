using ChitChat.Service.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace ChitChat.Web.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMemoryCache cache;
        private readonly int requestLimit = 5;
        private readonly TimeSpan timeWindow = TimeSpan.FromSeconds(10);

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            this.next = next;
            this.cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            if (ip == null)
            {
                await next(context);
                return;
            }
            var cacheKey = $"RateLimit_{ip}";
            var requestCount = cache.Get<int>(cacheKey);

            if (requestCount >= requestLimit)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new BaseResult<string>() { IsSuccess = false, Errors = ["Too many requests. Try again later."], StatusCode = MyStatusCode.TooManyRequests };
                response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
                return;
            }

            cache.Set(cacheKey, requestCount + 1, timeWindow);
            await next(context);
        }
    }
}
