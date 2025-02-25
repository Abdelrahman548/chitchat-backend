using ChitChat.Repository.Implementations;
using ChitChat.Repository.Interfaces;
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

            services.AddScoped<ServicesUnit>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOTPService, OTPService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<ICloudService, CloudinaryService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IGroupMessageService, GroupMessageService>();

        }
    }
}
