using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinecraftDiscordStatus.BLL.Services;
using MinecraftDiscordStatus.Shared.Configuration;

namespace MinecraftDiscordStatus.BLL
{
    public static class DIHelper
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            // Initialize configuration
            services.AddOptions();

            IConfiguration configuration = LoadConfiguration();
            services.AddSingleton(configuration);

            IConfigurationSection credentialsConfig = configuration.GetSection(ConfigurationSectionName.Credentials);

            services.Configure<CredentialsConfig>(credentialsConfig);

            // Register services
            services.AddScoped<IDiscordBotService, DiscordBotService>();

            return services;
        }

        private static IConfiguration LoadConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
