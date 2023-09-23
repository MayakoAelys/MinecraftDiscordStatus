using Microsoft.Extensions.DependencyInjection;
using MinecraftDiscordStatus.BLL.Services;

namespace MinecraftDiscordStatus
{
    public class App
    {
        private readonly IDiscordBotService _discordBotService;
        public App(IDiscordBotService discordBotService)
        {
            _discordBotService = discordBotService;
        }

        public void Start(IServiceCollection services)
        {
            _discordBotService
                .StartBot(services)
                .GetAwaiter()
                .GetResult();
        }
    }
}
