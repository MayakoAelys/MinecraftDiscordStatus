using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Text;

namespace MinecraftDiscordStatus.BLL.Services
{
    public class ButtonService : IButtonService
    {
        private IMinecraftService _minecraftService;

        public ButtonService(IMinecraftService minecraftService)
        {
            _minecraftService = minecraftService;
        }

        public async Task MinecraftUpdatePlayerCount(ComponentInteractionCreateEventArgs componentInteractionCreateEventArgs)
        {
            await componentInteractionCreateEventArgs.Interaction.DeferAsync(ephemeral: true);

            DiscordEmbedBuilder newEmbed =
                GeneratePlayersOnlineEmbed((DiscordMember) componentInteractionCreateEventArgs.User);

            await componentInteractionCreateEventArgs.Message.ModifyAsync(newEmbed.Build());

            // Delete the "bot is thinking" message
            await componentInteractionCreateEventArgs.Interaction.DeleteOriginalResponseAsync();
        }

        public DiscordEmbedBuilder GeneratePlayersOnlineEmbed(DiscordMember user)
        {
            var minestat = _minecraftService.GetMinecraftServerInfo();
            var embed = new DiscordEmbedBuilder();
            var stringBuilder = new StringBuilder();

            embed.WithColor(new DiscordColor("#ff0080"));

            // Server stats
            embed.WithTitle(":earth_africa: Minecraft server stats");

            stringBuilder.AppendLine($"- Online players: {minestat.CurrentPlayers} / {minestat.MaximumPlayers}");
            stringBuilder.AppendLine($"- Ping: {minestat.Latency}ms");

            embed.AddField(":satellite: General", stringBuilder.ToString());
            stringBuilder.Clear();

            // Online players
            if (minestat.PlayerList.Count() == 0)
            {
                BuildEmbed(ref embed, user);

                return embed;
            }

            foreach (string? player in minestat.PlayerList)
            {
                if (string.IsNullOrEmpty(player))
                    continue;

                stringBuilder.AppendLine($"- {player}");
            }

            embed.AddField(":eyes: Who is online?", stringBuilder.ToString());
            stringBuilder.Clear();

            BuildEmbed(ref embed, user);

            return embed;
        }

        private void BuildEmbed(ref DiscordEmbedBuilder embed, DiscordMember user)
        {
            embed.WithFooter(
                string.Concat(
                    $"🕒 Last updated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                    $"\n🐰 Asked by: {user.DisplayName} ({user.Username})"));

            embed.Build();
        }
    }
}
