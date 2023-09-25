using DSharpPlus.Entities;
using System.Text;
using DSharpPlus.SlashCommands;
using MineStatLib;
using DSharpPlus;

namespace MinecraftDiscordStatus.BLL.Commands
{
    public class InfoCommands : ApplicationCommandModule
    {
        private IMinecraftService _minecraftService;

        public InfoCommands(IMinecraftService minecraftService)
        {
            _minecraftService = minecraftService;
        }

        [SlashCommand("onlineplayers", "See who is currently online on the minecraft server")]
        public async Task ShowOnlinePlayers(InteractionContext interactionContext)
        {
            await interactionContext.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MineStat minestat = _minecraftService.GetMinecraftServerInfo();

            DiscordEmbedBuilder playersOnlineEmbed = GeneratePlayersOnlineEmbed(minestat);
            DiscordWebhookBuilder discordWebhookBuilder = new DiscordWebhookBuilder().AddEmbed(playersOnlineEmbed);

            await interactionContext.EditResponseAsync(discordWebhookBuilder);
        }

        private DiscordEmbedBuilder GeneratePlayersOnlineEmbed(MineStat minestat)
        {
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
                BuildEmbed(ref embed);

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

            BuildEmbed(ref embed);

            return embed;
        }

        private void BuildEmbed(ref DiscordEmbedBuilder embed)
        {
            embed.WithFooter("🥕 bunbun~");
            embed.Build();
        }
    }
}
