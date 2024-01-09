using Discord.Interactions;
using Tasky.Models;
using Tasky.Services;

namespace Tasky.Modules;

public class ConfigCommandModule : InteractionModuleBase<SocketInteractionContext>
{
    public IConfigStorage configs { get; set; }

    [SlashCommand("config", "현재 서버의 Tasky 설정을 불러옵니다")]
    public async Task GetConfigAsync()
    {
        var guild = Context.Guild.Id;

        if (configs.TryGetConfig(guild, out var config) && config != null)
        {
            await RespondAsync($"Channel ID: {config.ChannelID}\nTime: {config.Time.Hour}\nIgnore Weekend: {config.IgnoreWeekend}\nIgnore Holiday: {config.IgnoreHoliday}");
        }
        else
        {
            await RespondAsync($"설정이 없습니다! /set 명령어를 통해 설정을 추가해주세요");
        }
    }

    [Group("set", "쓰레드 설정을 수정합니다")]
    public class SetConfigGroup : InteractionModuleBase<SocketInteractionContext>
    {
        public IConfigStorage configs { get; set; }

        [SlashCommand("channel", "쓰레드를 생성할 채널을 설정합니다")]
        public async Task SetChannelAsync([Summary(name: "channelId", description: "설정할 채널의 id입니다 (없을 시 현재 채널의 id)")] string id = "")
        {
            var guild = Context.Guild.Id;
            var channelId = string.IsNullOrEmpty(id) ? Context.Channel.Id : Convert.ToUInt64(id);

            if (configs.TryGetConfig(guild, out var config) && config != null)
            {
                config.ChannelID = channelId;
                configs.SaveChanges();
            }
            else
            {
                var newConfig = new Config
                {
                    ChannelID = channelId
                };

                configs.TryAdd(guild, newConfig);
            }

            await RespondAsync($"쓰레드 생성 채널을 {channelId}로 설정합니다");
        }

        [SlashCommand("time", "쓰레드를 생성할 채널을 설정합니다")]
        public async Task SetTimeAsync([Summary(name: "time", description: "쓰레드를 생성할 시간입니다")] int hour)
        {
            var guild = Context.Guild.Id;

            if (hour < 0 || hour > 23)
            {
                await RespondAsync($"올바르지 않은 값입니다! (0부터 23사이로 입력해주세요)");
                return;
            }

            if (configs.TryGetConfig(guild, out var config) && config != null)
            {
                config.Time = DateTime.Today.AddHours(hour);
                configs.SaveChanges();
            }
            else
            {
                var newConfig = new Config
                {
                    Time = DateTime.Today.AddHours(hour)
                };

                configs.TryAdd(guild, newConfig);
            }

            await RespondAsync($"쓰레드 생성 시간을 {hour}시로 설정합니다");
        }

        [SlashCommand("ignore-weekend", "주말에 쓰레드를 생성할지 설정합니다")]
        public async Task SetIgnoreWeekendAsync(bool ignore)
        {
            var guild = Context.Guild.Id;

            if (configs.TryGetConfig(guild, out var config) && config != null)
            {
                config.IgnoreWeekend = ignore;
                configs.SaveChanges();
            }
            else
            {
                var newConfig = new Config
                {
                    IgnoreWeekend = ignore
                };

                configs.TryAdd(guild, newConfig);
            }

            await RespondAsync(ignore ? $"주말에 쓰레드를 생성합니다" : $"주말에 쓰레드를 생성하지 않습니다");
        }

        [SlashCommand("ignore-holiday", "공휴일에 쓰레드를 생성할지 설정합니다")]
        public async Task SetIgnoreHolidayAsync(bool ignore)
        {
            var guild = Context.Guild.Id;

            if (configs.TryGetConfig(guild, out var config) && config != null)
            {
                config.IgnoreHoliday = ignore;
                configs.SaveChanges();
            }
            else
            {
                var newConfig = new Config
                {
                    IgnoreHoliday = ignore
                };

                configs.TryAdd(guild, newConfig);
            }

            await RespondAsync(ignore ? $"공휴일에 쓰레드를 생성합니다" : $"공휴일에 쓰레드를 생성하지 않습니다");
        }
    }
}