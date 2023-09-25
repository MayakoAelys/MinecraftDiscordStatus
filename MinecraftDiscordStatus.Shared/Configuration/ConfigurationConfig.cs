namespace MinecraftDiscordStatus.Shared.Configuration
{
    public class ConfigurationConfig
    {
        public int RefreshTimeSeconds { get; set; }
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public string ChannelNameTemplate { get; set; }
        public string MinecraftServerIP { get; set; }
        public ushort MinecraftServerPort { get; set; }
    }
}