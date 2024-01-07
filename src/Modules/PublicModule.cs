using Discord.Interactions;

namespace Tasky.Modules;

public class PublicModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "pong!")]
    public Task PingAsync()
        => RespondAsync("pong!");

    [SlashCommand("get-server-id", "Get Server ID")]
    public Task GetServerIDAsync()
        => RespondAsync(Context.Guild.Id.ToString());
}