using DSharpPlus;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IPeriodicTaskService
    {
        void UpdatePlayerCount(DiscordClient discordClient);
    }
}