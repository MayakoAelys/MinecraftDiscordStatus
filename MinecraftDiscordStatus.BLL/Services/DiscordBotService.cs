using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.Shared.Configuration;
using MinecraftDiscordStatus.Shared.Resources;
using Serilog;

namespace MinecraftDiscordStatus.BLL.Services
{
    public class DiscordBotService : IDiscordBotService
    {
        private CredentialsConfig _credentialsConfig;
        private static DiscordClient _discordClient;

        public DiscordBotService(
            IOptions<CredentialsConfig> credentialConfig)
        {
            _credentialsConfig = credentialConfig?.Value;
        }

        public async Task StartBot(IServiceCollection services)
        {
            Log.Information(Messages.Internal_StartingBot);

            _discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = _credentialsConfig.Token,
                TokenType = TokenType.Bot
            });

            InitEvents(ref _discordClient);

            Log.Information(Messages.Internal_ConnectingToDiscord);
            await _discordClient.ConnectAsync();

            Log.Information(Messages.Internal_BotStarted);
            await Task.Delay(-1);
        }

        #region Private functions

        private void InitEvents(ref DiscordClient discordClient)
        {
            discordClient.ClientErrored += ClientErrored;
        }

        private Task ClientErrored(DiscordClient discordClient, ClientErrorEventArgs e)
        {
            Log.Error(e.Exception, Messages.Internal_ClientError);

            return Task.CompletedTask;
        }

        #endregion
    }
}
