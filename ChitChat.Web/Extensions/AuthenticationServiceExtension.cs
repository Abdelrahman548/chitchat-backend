using ChitChat.Service.Helpers;

namespace ChitChat.Web.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static void AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
            if (jwtOptions is not null)
            {

            }
        }
    }
}
