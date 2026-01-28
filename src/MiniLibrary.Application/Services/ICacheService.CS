namespace MiniLibrary.Application.Services;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T data, TimeSpan duration);
}