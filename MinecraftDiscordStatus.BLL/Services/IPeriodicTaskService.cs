using DSharpPlus;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IPeriodicTaskService
    {
        Task<string> UpdatePlayerCount(DiscordClient discordClient, string lastChannelName);
    }
}