using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tasky.Services;

public class HolidayInfoLoader
{
    private readonly IConfiguration _configure;
    private dynamic[] _data;
    private string _filename;

    public HolidayInfoLoader(IConfiguration configure)
    {
        _configure = configure;

        _filename = configure.GetConnectionString("holidays") ?? "holidays.json";

        try
        {
            var jsonData = File.ReadAllText(_filename);

            _data = JsonConvert.DeserializeObject<dynamic[]>(jsonData) ?? throw new InvalidDataException("Invalid json data");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

            _data = [];
        }
    }

    public bool IsHoliday(DateTime now)
    {
        foreach (var d in _data)
        {
            var date = new DateTime(d.solar_date).Date;

            if (date == DateTime.Today)
                return true;
        }

        return false;
    }
}