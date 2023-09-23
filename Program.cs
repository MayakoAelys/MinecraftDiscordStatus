using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.BLL;
using MinecraftDiscordStatus.Shared.Configuration;
using MinecraftDiscordStatus.Shared.Logging;
using Serilog;

namespace MinecraftDiscordStatus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services = DIHelper.ConfigureServices(services);

            // Add the App start to the services collection
            services.AddTransient<App>();

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var credentials = serviceProvider.GetService<IOptions<CredentialsConfig>>().Value;

                // Add Serilog logging
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(serviceProvider.GetService<IConfiguration>())
                    .Destructure.With<IncludePublicFieldsPolicy>()
                    .Enrich.WithCaller()
                    .CreateLogger();

                serviceProvider.GetService<App>().Start(services);
            }
        }
    }
}