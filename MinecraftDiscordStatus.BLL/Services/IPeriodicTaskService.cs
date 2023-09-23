using DSharpPlus;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IPeriodicTaskService
    {
        Task UpdatePlayerCount(DiscordClient discordClient);
    }
}