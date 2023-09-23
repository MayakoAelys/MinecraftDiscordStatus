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
            IConfigurationSection configurationConfig = configuration.GetSection(ConfigurationSectionName.Configuration);

            services.Configure<CredentialsConfig>(credentialsConfig);
            services.Configure<ConfigurationConfig>(configurationConfig);

            // Register services
            services.AddScoped<IDiscordBotService, DiscordBotService>();
            services.AddScoped<IPeriodicTaskService, PeriodicTaskService>();

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
