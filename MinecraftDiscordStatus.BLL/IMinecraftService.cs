using MineStatLib;

namespace MinecraftDiscordStatus.BLL
{
    public interface IMinecraftService
    {
        string GetMinecraftOnlinePlayers();
        MineStat GetMinecraftServerInfo();
    }
}