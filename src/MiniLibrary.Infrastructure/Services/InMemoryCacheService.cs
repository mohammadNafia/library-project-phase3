using System;
using System.Collections.Concurrent;
using MiniLibrary.Application.Services;

namespace MiniLibrary.Infrastructure.Services;
public class InMemoryCacheService : ICacheService
{
    private readonly Dictionary<string, (object Data, DateTime Expiry)> _cache
        = new();

    public T? Get<T>(string key)
    {
        if (!_cache.ContainsKey(key))
            return default;

        var item = _cache[key];
        if (item.Expiry < DateTime.UtcNow)
        {
            _cache.Remove(key);
            return default;
        }

        return (T)item.Data;
    }

    public void Set<T>(string key, T data, TimeSpan duration)
    {
        _cache[key] = (data!, DateTime.UtcNow.Add(duration));
    }
}
