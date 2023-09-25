using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace MinecraftDiscordStatus.BLL.Services
{
    public interface IButtonService
    {
        DiscordEmbedBuilder GeneratePlayersOnlineEmbed(DiscordMember user);
        Task MinecraftUpdatePlayerCount(ComponentInteractionCreateEventArgs componentInteractionCreateEventArgs);
    }
}