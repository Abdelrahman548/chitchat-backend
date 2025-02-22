using System.Text.Json;
using ChitChat.Service.Helpers;

namespace ChitChat.Web.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApiKeyMiddleware> logger;
        private readonly IConfiguration configuration;
        private const string API_KEY_HEADER = "API_KEY";


        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration configuration)
        {
            this.next = next;
            this.logger = logger;
            this.configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                var result = new BaseResult<string>() { IsSuccess = false, StatusCode = MyStatusCode.Unauthorized, Errors = ["API KEY is missing"] };
                var serilaizedResult = JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(serilaizedResult);
                return;
            }
            if (CheckApiKey(extractedApiKey))
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                var result = new BaseResult<string>() { IsSuccess = false, StatusCode = MyStatusCode.Forbidden, Errors = ["Invalid API KEY"] };
                var serilaizedResult = JsonSerializer.Serialize(result);
                await context.Response.WriteAsync(serilaizedResult);
                return;
            }
        }
        private bool CheckApiKey(string? key)
        {
            var MyKey = configuration["APIKEY"];
            return MyKey?.Equals(key) ?? false;
        }
    }
}
