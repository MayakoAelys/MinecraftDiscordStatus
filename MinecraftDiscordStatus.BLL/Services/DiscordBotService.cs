using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinecraftDiscordStatus.BLL.Commands;
using MinecraftDiscordStatus.Shared.Configuration;
using MinecraftDiscordStatus.Shared.Constants;
using MinecraftDiscordStatus.Shared.Resources;
using Serilog;

namespace MinecraftDiscordStatus.BLL.Services
{
    public class DiscordBotService : IDiscordBotService
    {
        private CredentialsConfig    _credentialsConfig;
        private ConfigurationConfig  _configurationConfig;
        private IPeriodicTaskService _periodicTaskService;
        private IMinecraftService    _minecraftService;
        private IButtonService       _buttonService;

        private static DiscordClient _discordClient;
        
        public DiscordBotService(
            IOptions<CredentialsConfig> credentialConfig,
            IPeriodicTaskService periodicTaskService,
            IOptions<ConfigurationConfig> configurationConfig,
            IMinecraftService minecraftService,
            IButtonService buttonService)
        {
            _credentialsConfig   = credentialConfig?.Value;
            _periodicTaskService = periodicTaskService;
            _configurationConfig = configurationConfig?.Value;
            _minecraftService    = minecraftService;
            _buttonService       = buttonService;
        }

        public async Task StartBot(IServiceCollection services)
        {
            Log.Information(Messages.Internal_StartingBot);

            _discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = _credentialsConfig.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = LogLevel.Information
            });

            InitEvents(ref _discordClient);

            Log.Information(Messages.Internal_ConnectingToDiscord);
            
            await _discordClient.ConnectAsync();
            
            Log.Information(Messages.Internal_BotStarted);

            InitializeCommands();

            await StartPeriodicTimer();

            // await Task.Delay(-1);
        }

        #region Private functions

        private void InitializeCommands()
        {
            SlashCommandsExtension slashCommands =
                _discordClient.UseSlashCommands(new SlashCommandsConfiguration
                {
                    Services = new ServiceCollection().AddSingleton<IButtonService>(_buttonService).BuildServiceProvider()
                }); 

            slashCommands.RegisterCommands<InfoCommands>(guildId: _configurationConfig.GuildId);
        }

        private async Task StartPeriodicTimer()
        {
            var _periodicTimer =
                new PeriodicTimer(TimeSpan.FromSeconds(_configurationConfig.RefreshTimeSeconds));

            string lastChannelName = string.Empty;

            while (await _periodicTimer.WaitForNextTickAsync())
            {
                lastChannelName = await _periodicTaskService.UpdatePlayerCount(_discordClient, lastChannelName);
            }
        }

        private void InitEvents(ref DiscordClient discordClient)
        {
            discordClient.ClientErrored += ClientErrored;
            discordClient.ComponentInteractionCreated += ComponentInteractionCreated;
        }

        private Task ClientErrored(DiscordClient discordClient, ClientErrorEventArgs e)
        {
            Log.Error(e.Exception, Messages.Internal_ClientError);

            return Task.CompletedTask;
        }

        private Task ComponentInteractionCreated(
            DiscordClient discordClient,
            ComponentInteractionCreateEventArgs componentInteractionCreateEventArgs)
        {
            switch(componentInteractionCreateEventArgs.Id)
            {
                case DiscordButtonId.MinecraftUpdatePlayerCount:
                    _buttonService.MinecraftUpdatePlayerCount(componentInteractionCreateEventArgs);
                    break;

                default: // Should not happen
                    break;
            }

            return Task.CompletedTask;
        }

        #endregion
    }
}
