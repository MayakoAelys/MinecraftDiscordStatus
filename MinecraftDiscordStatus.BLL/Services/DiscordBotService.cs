using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.Shared.Configuration;
using MinecraftDiscordStatus.Shared.Resources;
using Serilog;
using System.Threading;

namespace MinecraftDiscordStatus.BLL.Services
{
    public class DiscordBotService : IDiscordBotService
    {
        private IPeriodicTaskService _periodicTaskService;
        private CredentialsConfig _credentialsConfig;
        private static DiscordClient _discordClient;

        public DiscordBotService(
            IOptions<CredentialsConfig> credentialConfig,
            IPeriodicTaskService periodicTaskService)
        {
            _credentialsConfig = credentialConfig?.Value;
            _periodicTaskService = periodicTaskService;
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

            var _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await _periodicTimer.WaitForNextTickAsync())
            {
                Log.Debug("1 second loop");
            }

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
