using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Tasky;

public class Program
{
    DiscordSocketClient _client = null!; //봇 클라이언트
    CommandService _commands = null!;    //명령어 수신 클라이언트

    /// <summary>
    /// 프로그램의 진입점
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var section = config.GetSection("Environments");

        try
        {
            var token = section["Token"] ?? throw new InvalidOperationException("Token string was not found.");
            new Program().BotMain(token).GetAwaiter().GetResult();   //봇의 진입점 실행
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// 봇의 진입점, 봇의 거의 모든 작업이 비동기로 작동되기 때문에 비동기 함수로 생성해야 함
    /// </summary>
    /// <returns></returns>
    public async Task BotMain(string token)
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        _commands = new CommandService(new CommandServiceConfig()
        {
            LogLevel = LogSeverity.Verbose
        });

        //로그 수신 시 로그 출력 함수에서 출력되도록 설정
        _client.Log += OnClientLogReceived;
        _commands.Log += OnClientLogReceived;

        await _client.LoginAsync(TokenType.Bot, token); //봇의 토큰을 사용해 서버에 로그인
        await _client.StartAsync();                         //봇이 이벤트를 수신하기 시작

        _client.MessageReceived += OnClientMessage;         //봇이 메시지를 수신할 때 처리하도록 설정

        await Task.Delay(-1);   //봇이 종료되지 않도록 블로킹
    }

    private async Task OnClientMessage(SocketMessage arg)
    {
        //수신한 메시지가 사용자가 보낸 게 아닐 때 취소
        var message = arg as SocketUserMessage;
        if (message == null) return;

        if (message.Author.Id == _client.CurrentUser.Id)
            return;

        Console.WriteLine(message.Content);

        await message.Channel.SendMessageAsync("pong!");
    }

    /// <summary>
    /// 봇의 로그를 출력하는 함수
    /// </summary>
    /// <param name="msg">봇의 클라이언트에서 수신된 로그</param>
    /// <returns></returns>
    private Task OnClientLogReceived(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());  //로그 출력
        return Task.CompletedTask;
    }
}