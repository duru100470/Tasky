using Discord;
using Discord.WebSocket;

namespace Tasky.Services;

public class TestService
{
    private readonly DiscordSocketClient _discord;
    private readonly IServiceProvider _services;

    public TestService(DiscordSocketClient discord, IServiceProvider services)
    {
        _discord = discord;
        _services = services;

        _discord.MessageReceived += OnMessageReceivedAsync;
    }

    public async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage)
            return;
        if (message.Source != MessageSource.User)
            return;

        await message.Channel.SendMessageAsync("test");
    }
}