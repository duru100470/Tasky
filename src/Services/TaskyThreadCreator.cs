using Discord;
using Discord.WebSocket;

namespace Tasky.Services;

public class TaskyThreadCreator
{
    private readonly DiscordSocketClient _discord;
    private readonly IConfigStorage _storage;
    private int _hour;

    public TaskyThreadCreator(DiscordSocketClient discord, IConfigStorage storage)
    {
        _discord = discord;
        _storage = storage;
        _hour = DateTime.Now.Hour;
    }

    public void Initialize()
    {
        _discord.Ready += Ready;
    }

    private async Task Ready()
    {
        Thread thread = new Thread(TimerFunction);
        thread.Start();
        Console.WriteLine("Timer starts!");
    }

    private async Task CreateThread()
    {
        var guilds = _discord.Guilds.Select(g => g.Id);

        foreach (var guild in guilds)
        {
            if (_storage.TryGetConfig(guild, out var config))
            {
                if (config.Time.Hour == _hour)
                {
                    var channel = _discord.GetChannel(config.ChannelID) as ITextChannel;

                    if (channel == null)
                        continue;

                    // Create a public thread with the given name and auto-archive duration
                    var thread = await channel.CreateThreadAsync(DateTime.Now.ToString("d"), ThreadType.PublicThread, ThreadArchiveDuration.OneDay);

                    // Send a message in the thread
                    await thread.SendMessageAsync("오늘 할 작업을 적어주세요");
                }
            }
        }
    }

    private void TimerFunction()
    {
        while (true)
        {
            if (DateTime.Now.Hour != _hour)
            {
                _hour = DateTime.Now.Hour;
                _ = CreateThread();
            }
            Thread.Sleep(60000);
        }
    }
}