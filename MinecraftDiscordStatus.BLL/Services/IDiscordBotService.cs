using Microsoft.Extensions.DependencyInjection;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IDiscordBotService
    {
        Task StartBot(IServiceCollection services);
    }
}