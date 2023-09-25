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
        private IMinecraftService _minecraftService;

        public PeriodicTaskService(
            IOptions<ConfigurationConfig> configurationConfig,
            IMinecraftService minecraftService)
        {
            _configurationConfig = configurationConfig?.Value;
            _minecraftService    = minecraftService;
        }

        public async Task<string> UpdatePlayerCount(DiscordClient discordClient, string lastChannelName)
        {
            Log.Debug($"UpdatePlayerCount | lastChannelName: {lastChannelName}");

            try
            {
                string channelName = _configurationConfig.ChannelNameTemplate;
                string onlinePlayers = _minecraftService.GetMinecraftOnlinePlayers();

                channelName = string.Format(channelName, onlinePlayers);

                Log.Debug($"lastChannelName: {lastChannelName} | channelName: {channelName} | Equals: {channelName.Equals(lastChannelName)}");

                // Avoid setting the channel name again if it is not necessary (to avoid being rate limited)
                if (!string.IsNullOrEmpty(lastChannelName) && channelName.Equals(lastChannelName))
                {
                    return lastChannelName;
                }

                DiscordChannel channel =
                        await discordClient.GetChannelAsync(_configurationConfig.ChannelId);

                Log.Debug($"Trying to update the channel name to '{channelName}'");

                await channel.ModifyAsync(prop => prop.Name = channelName);

                return channelName;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occured while trying to update the player count.");

                string channelName = await TrySetChannelNameSafely(discordClient);

                return channelName;
            }
        }

        private async Task<string> TrySetChannelNameSafely(DiscordClient discordClient)
        {
            string channelName = _configurationConfig.ChannelNameTemplate;
            
            channelName = string.Format(channelName, "N/A");

            try
            {
                DiscordChannel channel =
                    await discordClient.GetChannelAsync(_configurationConfig.ChannelId);

                await channel.ModifyAsync(prop => prop.Name = channelName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception also occured while trying to change the channel name safely");
            }

            return channelName;
        }
    }
}
