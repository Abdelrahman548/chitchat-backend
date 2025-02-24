using ChitChat.Service.Helpers;
using DotNetEnv;

namespace ChitChat.Web.Extensions
{
    public static class OptionsServiceExtension
    {
        public static void AddOptionsService(this IServiceCollection services, IConfiguration configuration)
        {
            //// Loading Environment Variables ////
            Env.Load();

            var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            var APIKey = Environment.GetEnvironmentVariable("APIKEY");

            var mongoConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTIONSTRING");
            var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASENAME");
            
            var JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var JwtLifeTime = Environment.GetEnvironmentVariable("JWT_LIFETIME");
            var JwtSigningKey = Environment.GetEnvironmentVariable("JWT_SIGNINGKEY");
            
            var Smtp = Environment.GetEnvironmentVariable("EMAIL_SMTP");
            var SenderName = Environment.GetEnvironmentVariable("EMAIL_SENDER_NAME");
            var SenderEmail = Environment.GetEnvironmentVariable("EMAIL_SENDER_EMAIL");
            var AppPass = Environment.GetEnvironmentVariable("EMAIL_APP_PASS");
            var PortSSL = Environment.GetEnvironmentVariable("EMAIL_PORT_SSL");
            var Port = Environment.GetEnvironmentVariable("EMAIL_PORT");

            //// Assign Variables to AppSettings json file ////
            configuration["CLOUDINARY_URL"] = cloudinaryUrl;
            configuration["APIKEY"] = APIKey;

            configuration["MongoDB:ConnectionString"] = mongoConnectionString;
            configuration["MongoDB:DatabaseName"] = mongoDatabaseName;
            
            configuration["Jwt:Issuer"] = JwtIssuer;
            configuration["Jwt:Audience"] = JwtAudience;
            configuration["Jwt:LifeTime"] = JwtLifeTime;
            configuration["Jwt:SigningKey"] = JwtSigningKey;

            configuration["Email:SmtpServer"] = Smtp;
            configuration["Email:SenderEmail"] = SenderEmail;
            configuration["Email:SenderName"] = SenderName;
            configuration["Email:AppPassword"] = AppPass;
            configuration["Email:PortSSL"] = PortSSL;
            configuration["Email:Port"] = Port;


            //// Load Options ////
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
            if (jwtOptions is not null)
                services.AddSingleton(jwtOptions);

            var MongoOptions = configuration.GetSection("MongoDB").Get<MongoDBOptions>();
            if (MongoOptions is not null)
                services.AddSingleton(MongoOptions);

            var emailOptions = configuration.GetSection("Email").Get<EmailOptions>();
            if (emailOptions is not null)
                services.AddSingleton(emailOptions);
        }
    }
}
