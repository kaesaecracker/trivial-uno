using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

class LoggingManager
{
    public static readonly ILoggerFactory Factory = LoggerFactory.Create(ConfigureLoggingBuilder);

    private static void ConfigureLoggingBuilder(ILoggingBuilder builder)
    {
        builder.SetMinimumLevel(LogLevel.Trace)
            .AddFilter("System", LogLevel.Warning)
            .AddFilter("TrivialUno.PlayerTurnOder", LogLevel.Information)
            .AddSimpleConsole(ConfigureConsole);
    }

    private static void ConfigureConsole(SimpleConsoleFormatterOptions opts)
    {
        opts.IncludeScopes = true;
        opts.SingleLine = true;
    }
}