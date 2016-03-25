using System;
using DataGenericCache.Exceptions;
using DataGenericCache.Extensions;
using DataGenericCache.Settings;
using StackExchange.Redis;

namespace DataGenericCache.Providers
{
    internal class RedisCacheProvider : ICacheProvider
    {
        private static Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private static readonly object Locker = new object();

        private IDatabase RedisDatabase => Connection.GetDatabase();

        private static ConnectionMultiplexer Connection
        {
            get
            {
                try
                {
                    return _connectionMultiplexer.Value;
                }
                catch (Exception)
                {
                    throw new CacheProviderConnectionException();
                }
            }
        }

        public void SetupConfiguration(ServerSettings serverSettings)
        {
            lock (Locker)
            {
                if (_connectionMultiplexer == null)
                {
                    var configurationOptions = GetConfigurationOptions(serverSettings);
                    _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
                }
            }
        }

        private ConfigurationOptions GetConfigurationOptions(ServerSettings serverSettings)
        {
            var connectionString = GetConnectionString(serverSettings);
            var configurationOptions = ConfigurationOptions.Parse(connectionString);

            if (!string.IsNullOrWhiteSpace(serverSettings.Password))
            { 
                configurationOptions.Password = serverSettings.Password;
            }

            return configurationOptions;
        }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            RedisDatabase.StringSet(key, value.ToJson(), expiration, When.NotExists);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            RedisDatabase.StringSet(key, value.ToJson(), expiration, When.Exists);
        }

        public void Remove(string key)
        {
            RedisDatabase.KeyDelete(key);
        }

        public T Retrieve<T>(string key)
        {
            var obtained = RedisDatabase.StringGet(key);

            if (obtained.IsNull)
            { 
                return default(T);
            }

            return RedisDatabase
                .StringGet(key)
                .ToString()
                .FromJson<T>();
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

        public bool Exists(string key)
        {
            return RedisDatabase.KeyExists(key);
        }

        private string GetConnectionString(ServerSettings serverSettings)
        {
            return $"{serverSettings.Address}:{serverSettings.Port}";
        }
    }
}