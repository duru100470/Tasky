using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tasky.Services;

public class HolidayInfoLoader
{
    private readonly IConfiguration _configure;
    private HolidayInfo[] _data;
    private string _filename;

    public HolidayInfoLoader(IConfiguration configure)
    {
        _configure = configure;

        _filename = configure.GetConnectionString("holidays") ?? "holidays.json";

        try
        {
            var jsonData = File.ReadAllText(_filename);

            _data = JsonConvert.DeserializeObject<HolidayInfo[]>(jsonData) ?? throw new InvalidDataException("Invalid json data");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

            _data = [];
        }
    }

    public bool IsHoliday(DateTime now)
    {
        foreach (var info in _data)
        {
            var date = Convert.ToDateTime(info.solar_date);

            if (date == now.Date)
                return true;
        }

        return false;
    }

    private class HolidayInfo
    {
        public string memo = "";
        public string num = "";
        public string solar_date = "";
    }
}