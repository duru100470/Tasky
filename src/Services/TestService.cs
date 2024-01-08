using Discord;
using Discord.WebSocket;

namespace Tasky.Services;

public class TestService
{
    private readonly DiscordSocketClient _discord;

    public TestService(DiscordSocketClient discord)
    {
        _discord = discord;

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