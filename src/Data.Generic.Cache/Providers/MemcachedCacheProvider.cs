using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Data.Generic.Cache.Settings;
using Memcached.ClientLibrary;

namespace Data.Generic.Cache.Providers
{
    public class MemcachedCacheProvider : ICacheProvider
    {
        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public T Retrieve<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T RetrieveOrElse<T>(string key, TimeSpan expiration, Func<T> retrievalDelegate)
        {
            throw new NotImplementedException();
        }

        public void SetupConfiguration(ServerSettings serverSettings)
        {
            Memcached.ClientLibrary.MemcachedClient client = new MemcachedClient()
            {

            };
        }

        public bool IsWorking()
        {
            throw new NotImplementedException();
        }
    }
}