using Tasky.Models;

namespace Tasky.Services;

public class InMemoryConfigStorage : IConfigStorage
{
    private Dictionary<ulong, Config> _storage = new();

    public void TryAdd(ulong guild, Config config)
    {
        _storage[guild] = config;
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
}