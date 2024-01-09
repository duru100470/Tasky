using Tasky.Models;

namespace Tasky.Services;

public interface IConfigStorage
{
    bool TryGetConfig(ulong guild, out Config? config);
    void TryAdd(ulong guild, Config config);
    void SaveChanges();
}