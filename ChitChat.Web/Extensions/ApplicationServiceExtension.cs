using ChitChat.Service.Implementations;
using ChitChat.Service.Interfaces;
using ChitChat.Service.Profiles;

namespace ChitChat.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            // services.AddAutoMapper(typeof(xxxxProfile));
            // services.AddScoped<IxxxxService, xxxxService>();

            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(MessageProfile));
            services.AddAutoMapper(typeof(StatusProfile));
            services.AddAutoMapper(typeof(GroupProfile));

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IOTPService, OTPService>();
            services.AddSingleton<IStatusService, StatusService>();
            services.AddSingleton<IFriendService, FriendService>();
            services.AddSingleton<ICloudService, CloudinaryService>();
            services.AddSingleton<IChatMessageService, ChatMessageService>();
            services.AddSingleton<IGroupMessageService, GroupMessageService>();

        }
    }
}
