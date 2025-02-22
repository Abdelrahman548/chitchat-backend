using System.Text.Json;
using ChitChat.Service.Helpers;

namespace ChitChat.Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new BaseResult<string>() { IsSuccess = false, Errors = [e?.Message], StatusCode = MyStatusCode.InternalServerError };
                response.StatusCode = (int)MyStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
