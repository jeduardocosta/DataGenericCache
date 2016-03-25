using System;
using System.Web;
using System.Web.Caching;
using DataGenericCache.Exceptions;

namespace DataGenericCache.Providers
{
    internal class LocalMemoryCacheProvider : ICacheProvider
    {
        private static System.Web.Caching.Cache Cache
        {
            get
            {
                try
                {
                    return HttpRuntime.Cache;
                }
                catch (Exception)
                {
                    throw new CacheProviderConnectionException();
                }
            }
        }

        public void SetupConfiguration(Settings.ServerSettings serverSettings)
        { }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            Cache.Add(key, value, default(CacheDependency), DateTime.MaxValue, expiration, CacheItemPriority.High, default(CacheItemRemovedCallback));
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            Remove(key);
            Add(key, value, expiration);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return Cache[key] != null;
        }

        public T Retrieve<T>(string key)
        {
            return (T)Cache[key];
        }

        public T RetrieveOrElse<T>(string key, TimeSpan expiration, Func<T> retrievalDelegate)
        {
            var cachedObject = Cache[key];

            if (cachedObject == null)
            {
                var retrievedObject = retrievalDelegate();
                Add(key, retrievedObject, expiration);
                cachedObject = retrievedObject;
            }

            return (T)cachedObject;
        }
    }
}