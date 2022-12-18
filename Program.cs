using Microsoft.Extensions.Hosting;

[assembly: CLSCompliant(true)]

namespace TrivialUno;

public static class Program
{
    public static async Task Main(string[]? args)
    {
        using var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((HostBuilderContext _, IServiceCollection services) => services
                .AddLogging(Startup.ConfigureLoggingBuilder)
                .AddSingleton(_ => new Random(0)) // repoducability?
                .AddGame()
            )
            .Build();

        await host.RunAsync().ConfigureAwait(false);
    }
}
