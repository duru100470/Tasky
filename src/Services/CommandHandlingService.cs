using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tasky.Services;

public class CommandHandlingService
{
    private readonly DiscordSocketClient _discord;
    private readonly IServiceProvider _services;
    private readonly InteractionService _commands;
    private readonly IConfiguration _configuration;

    public CommandHandlingService(IServiceProvider service)
    {
        _services = service;
        _discord = service.GetRequiredService<DiscordSocketClient>();
        _commands = service.GetRequiredService<InteractionService>();
        _configuration = service.GetRequiredService<IConfiguration>();
    }

    public async Task InitializeAsync()
    {
        _discord.Ready += ReadyAsync;

        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _discord.InteractionCreated += HandleInteraction;
    }

    private async Task ReadyAsync()
    {
        var guild = _configuration.GetRequiredSection("Environments")["TestServer"];
        await _commands.RegisterCommandsToGuildAsync(Convert.ToUInt64(guild), true);
        // await _handler.RegisterCommandsGloballyAsync(true);
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_discord, interaction);

            // Execute the incoming command.
            var result = await _commands.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    default:
                        break;
                }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }
}