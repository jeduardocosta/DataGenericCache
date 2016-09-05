using System;
using System.Collections.Concurrent;
using System.Linq;
using DataGenericCache.Providers.Entities;

namespace DataGenericCache.Providers
{
    internal class LocalMemoryCacheProvider : ICacheProvider
    {
        private static readonly ConcurrentDictionary<string, object> Cache;

        static LocalMemoryCacheProvider()
        {
            Cache = new ConcurrentDictionary<string, object>();
        }

        public void SetupConfiguration(Settings.ServerSettings serverSettings)
        {
        }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            var data = new MemoryData<T>(value, expiration);
            Cache.TryAdd(key.ToLower(), data);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            Remove(key);
            Add(key, value, expiration);
        }

        public void Remove(string key)
        {
            object value;
            Cache.TryRemove(key.ToLower(), out value);
        }

        public bool Exists(string key)
        {
            return Cache.Any(e => e.Key == key.ToLower());
        }

        public T Retrieve<T>(string key)
        {
            var data = Cache
                .Where(e => e.Key == key.ToLower())
                .Select(e => e.Value)
                .FirstOrDefault() as MemoryData<T>;

            if (data == null)
            {
                return default(T);
            }

            if (data.IsExpired())
            {
                Remove(key);
                return default(T);
            }

            return data.Value;
        }

        public T RetrieveOrElse<T>(string key, TimeSpan expiration, Func<T> retrievalDelegate)
        {
            var cachedObject = Retrieve<T>(key);

            if (cachedObject == null)
            {
                var retrievedObject = retrievalDelegate();
                Add(key, retrievedObject, expiration);
                cachedObject = retrievedObject;
            }

            return cachedObject;
        }
    }
}