using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.Shared.Configuration;
using MineStatLib;
using Serilog;

namespace MinecraftDiscordStatus.BLL.Services
{
    public class PeriodicTaskService : IPeriodicTaskService
    {
        private ConfigurationConfig _configurationConfig;

        public PeriodicTaskService(IOptions<ConfigurationConfig> configurationConfig)
        {
            _configurationConfig = configurationConfig?.Value;
        }

        public async Task UpdatePlayerCount(DiscordClient discordClient)
        {
            try
            {
                string channelName = _configurationConfig.ChannelNameTemplate;

                DiscordChannel channel =
                    await discordClient.GetChannelAsync(_configurationConfig.ChannelId);

                string onlinePlayers = GetMinecraftOnlinePlayers();

                channelName = string.Format(channelName, onlinePlayers);

                await channel.ModifyAsync(prop => prop.Name = channelName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured while trying to update the player count.");
                
                await TrySetChannelNameSafely(discordClient);
            }
        }

        private string GetMinecraftOnlinePlayers()
        {
            var mineStat =
                new MineStat(
                    _configurationConfig.MinecraftServerIP,
                    _configurationConfig.MinecraftServerPort,
                    timeout: 5,
                    SlpProtocol.Json);

            if (!mineStat.ServerUp)
                return "Down";

            return $"{mineStat.CurrentPlayers}/{mineStat.MaximumPlayers}";
        }

        private async Task TrySetChannelNameSafely(DiscordClient discordClient)
        {
            try
            {
                string channelName = _configurationConfig.ChannelNameTemplate;

                DiscordChannel channel =
                    await discordClient.GetChannelAsync(_configurationConfig.ChannelId);

                channelName = string.Format(channelName, "N/A");

                await channel.ModifyAsync(prop => prop.Name = channelName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception also occured while trying to change the channel name safely");
            }
        }
    }
}
