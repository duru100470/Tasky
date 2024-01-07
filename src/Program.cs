using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services;

namespace Tasky;

public class Program
{
    static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

    /// <summary>
    /// 봇의 진입점, 봇의 거의 모든 작업이 비동기로 작동되기 때문에 비동기 함수로 생성해야 함
    /// </summary>
    /// <returns></returns>
    public async Task MainAsync()
    {
        using (var services = ConfigureServices())
        {
            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += OnClientLogReceived;
            services.GetRequiredService<InteractionService>().Log += OnClientLogReceived;

            var token = services.GetRequiredService<IConfiguration>().GetRequiredSection("Environments")["Token"]
                ?? throw new InvalidOperationException("Token was not found.");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
            services.GetRequiredService<TestService>();

            await Task.Delay(Timeout.Infinite);
        }
    }

    private Task OnClientLogReceived(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());  //로그 출력
        return Task.CompletedTask;
    }

    private ServiceProvider ConfigureServices()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            })
            .AddSingleton(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            })
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<InteractionService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<TestService>()
            .BuildServiceProvider();
    }
}