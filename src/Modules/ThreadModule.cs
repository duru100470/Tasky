using Discord;
using Discord.Interactions;
using Tasky.Services;

namespace Tasky.Modules;

public class ThreadModule : InteractionModuleBase<SocketInteractionContext>
{
    public IConfigStorage configs { get; set; }

    [SlashCommand("create", "서버에 오늘자 Tasky 쓰레드를 생성합니다")]
    public async Task CreateThreadAsync()
    {
        var guild = Context.Guild.Id;

        if (configs.TryGetConfig(guild, out var config) && config != null)
        {
            // Get the current channel from the command context
            var channel = Context.Guild.GetChannel(config.ChannelID) as ITextChannel;

            // Create a public thread with the given name and auto-archive duration
            var thread = await channel.CreateThreadAsync(DateTime.Now.ToString("d"), ThreadType.PublicThread, ThreadArchiveDuration.OneDay);

            // Send a message in the thread
            await thread.SendMessageAsync("오늘 할 작업을 적어주세요");
            await RespondAsync($"쓰레드를 성공적으로 생성했습니다!");
            return;
        }
        else
        {
            await RespondAsync($"설정이 없습니다! /set 명령어를 통해 설정을 추가해주세요");
            return;
        }
    }
}