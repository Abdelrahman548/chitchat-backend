using ChitChat.Service.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChitChat.Web.Extensions
{
    public static class DbServiceExtension
    {
        public static void AddDbService(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            services.Configure<MongoDBOptions>(configuration.GetSection("MongoDB"));
            var mongoSettings = configuration.GetSection("MongoDB").Get<MongoDBOptions>(); ;
            MongoClient? mongoClient = null;
            if (mongoSettings is not null)
            {
                try
                {
                    mongoClient = new MongoClient(mongoSettings.ConnectionString);

                    logger.LogInformation("MongoDB connected successfully using ConnectionString: {ConnectionString}, Database: {DatabaseName}", mongoSettings.ConnectionString, mongoSettings.DatabaseName);

                    services.AddSingleton<IMongoClient>(sp =>
                    {
                        return mongoClient;
                    });

                    services.AddSingleton(sp =>
                    {
                        var settings = sp.GetRequiredService<IOptions<MongoDBOptions>>().Value;
                        var client = sp.GetRequiredService<IMongoClient>();
                        return client.GetDatabase(settings.DatabaseName);
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to connect to MongoDB using ConnectionString: {ConnectionString}, Database: {DatabaseName}", mongoSettings.ConnectionString, mongoSettings.DatabaseName);
                }
            }
            else
            {
                logger.LogError("MongoDB settings are null. Please check your configuration.");
            }
        }
    }
}
