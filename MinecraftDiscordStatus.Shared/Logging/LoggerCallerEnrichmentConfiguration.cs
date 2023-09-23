using Serilog;
using Serilog.Configuration;

namespace MinecraftDiscordStatus.Shared.Logging
{
    // ref.: https://gist.github.com/nblumhardt/0e1e22f50fe79de60ad257f77653c813
    public static class LoggerCallerEnrichmentConfiguration
    {
        public static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            return enrichmentConfiguration.With<CallerEnricher>();
        }
    }
}
