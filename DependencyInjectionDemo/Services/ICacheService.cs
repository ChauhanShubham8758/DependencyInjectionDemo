using System.Collections.Concurrent;

namespace DependencyInjectionDemo.Services
{
    public interface ICacheService
    {
        void Add(string key, object value);
        object Get(string key);
    }

    public class SingletonCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, object> _cache = new();

        public void Add(string key, object value) => _cache.TryAdd(key, value);

        public object Get(string key) => _cache.TryGetValue(key, out var value) ? value : null;
    }
}
