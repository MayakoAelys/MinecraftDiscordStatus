using MineStatLib;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IMinecraftService
    {
        string GetMinecraftOnlinePlayers();
        MineStat GetMinecraftServerInfo();
    }
}