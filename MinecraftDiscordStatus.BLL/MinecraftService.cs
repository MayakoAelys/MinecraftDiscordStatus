using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.Shared.Configuration;
using MineStatLib;

namespace MinecraftDiscordStatus.BLL
{
    public class MinecraftService : IMinecraftService
    {
        private ConfigurationConfig _configurationConfig;

        public MinecraftService(IOptions<ConfigurationConfig> configurationConfig)
        {
            _configurationConfig = configurationConfig.Value;
        }

        public MineStat GetMinecraftServerInfo()
        {
            var mineStat =
                new MineStat(
                _configurationConfig.MinecraftServerIP,
                       _configurationConfig.MinecraftServerPort,
                       timeout: 5,
                       SlpProtocol.Json);

            return mineStat;
        }

        public string GetMinecraftOnlinePlayers()
        {
            var mineStat = GetMinecraftServerInfo();

            if (!mineStat.ServerUp)
                return "Down";

            return $"{mineStat.CurrentPlayers}/{mineStat.MaximumPlayers}";
        }
    }
}
