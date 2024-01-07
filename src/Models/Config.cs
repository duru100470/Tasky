namespace Tasky.Models;

public class Config
{
    public ulong ChannelID = 0;
    public DateTime Time = DateTime.Today.AddHours(10);
    public bool IgnoreWeekend = false;
    public bool IgnoreHoliday = false;
}