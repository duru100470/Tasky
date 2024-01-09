using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tasky.Models;

namespace Tasky.Services;

public class JsonConfigStorage : IConfigStorage
{
    private readonly IConfiguration _configure;
    private Dictionary<ulong, Config> _storage = new();
    private string _filename;

    public JsonConfigStorage(IConfiguration configure)
    {
        _configure = configure;

        string? filename = _configure.GetConnectionString("json");

        if (string.IsNullOrEmpty(filename))
            filename = "configs.json";

        _filename = filename;

        LoadConfigs();
    }

    public void TryAdd(ulong guild, Config config)
    {
        _storage[guild] = config;
        SaveConfigs();
    }

    public bool TryGetConfig(ulong guild, out Config? config)
    {
        if (_storage.TryGetValue(guild, out var value))
        {
            config = value;
            return true;
        }

        config = default;
        return false;
    }

    private void LoadConfigs()
    {
        try
        {
            var jsonString = File.ReadAllText(_filename);

            _storage = JsonConvert.DeserializeObject<Dictionary<ulong, Config>>(jsonString) ?? throw new InvalidDataException("Invalid json string");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void SaveConfigs()
    {
        try
        {
            var jsonString = JsonConvert.SerializeObject(_storage);

            File.WriteAllText(_filename, jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void SaveChanges()
        => SaveConfigs();
}