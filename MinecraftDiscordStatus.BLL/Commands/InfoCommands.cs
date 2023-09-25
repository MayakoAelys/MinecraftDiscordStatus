using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using MinecraftDiscordStatus.BLL.Services;
using MinecraftDiscordStatus.Shared.Constants;

namespace MinecraftDiscordStatus.BLL.Commands
{
    public class InfoCommands : ApplicationCommandModule
    {
        private IButtonService _buttonService;

        public InfoCommands(IButtonService buttonService)
        {
            _buttonService = buttonService;
        }

        [SlashCommand("onlineplayers", "See who is currently online on the minecraft server")]
        public async Task ShowOnlinePlayers(InteractionContext interactionContext)
        {
            await interactionContext.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            DiscordEmbedBuilder playersOnlineEmbed = 
                _buttonService.GeneratePlayersOnlineEmbed((DiscordMember) interactionContext.User);

            var discordWebhookBuilder = new DiscordWebhookBuilder();
            discordWebhookBuilder.AddEmbed(playersOnlineEmbed);

            var updateButton =
                new DiscordButtonComponent(
                    ButtonStyle.Primary,
                    DiscordButtonId.MinecraftUpdatePlayerCount,
                    "Update");

            discordWebhookBuilder.AddComponents(updateButton);

            await interactionContext.EditResponseAsync(discordWebhookBuilder);
        }
    }
}
