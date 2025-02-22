using ChitChat.Service.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Extensions
{
    public static class ModelStateResponseServiceExtension
    {
        public static void AddModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .SelectMany(x => x.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new BaseResult<string>
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors,
                        StatusCode = MyStatusCode.BadRequest
                    };

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
