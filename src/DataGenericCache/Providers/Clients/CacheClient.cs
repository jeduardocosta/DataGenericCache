using System;
using DataGenericCache.Exceptions;
using DataGenericCache.Providers.Factories;
using DataGenericCache.Settings;

namespace DataGenericCache.Providers.Clients
{
    public class CacheClient : ICacheProvider
    {
        private readonly ICacheProviderFactory _cacheProviderFactory;
        
        private ICacheProvider _cacheProvider;
        private int _attempts;

        public CacheClient() : this(new CacheProviderFactory())
        { }

        internal CacheClient(ICacheProviderFactory cacheProviderFactory)
        {
            _cacheProviderFactory = cacheProviderFactory;
            _cacheProvider = _cacheProviderFactory.Create();
        }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            ExecuteRequest(() => _cacheProvider.Add(key, value, expiration));
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            ExecuteRequest(() => _cacheProvider.Set(key, value, expiration));
        }

        public void Remove(string key)
        {
            ExecuteRequest(() => _cacheProvider.Remove(key));
        }

        public bool Exists(string key)
        {
            return ProcessRequest(() => _cacheProvider.Exists(key));
        }

        public T Retrieve<T>(string key)
        {
            return ProcessRequest(() => _cacheProvider.Retrieve<T>(key));
        }

        public T RetrieveOrElse<T>(string key, TimeSpan expiration, Func<T> retrievalDelegate)
        {
            return ProcessRequest(() => _cacheProvider.RetrieveOrElse(key, expiration, retrievalDelegate));
        }

        public void SetupConfiguration(ServerSettings serverSettings)
        {
            ExecuteRequest(() => _cacheProvider.SetupConfiguration(serverSettings));
        }

        private T ProcessRequest<T>(Func<T> functionRequest)
        {
            var result = default(T);

            try
            {
                result = functionRequest();
            }
            catch (CacheProviderConnectionException)
            {
                if (_attempts++ < _cacheProviderFactory.GetTotalAvailableProviders())
                {
                    _cacheProvider = _cacheProviderFactory.Create();
                    ProcessRequest(functionRequest);
                }
            }
            catch (Exception)
            { }

            return result;
        }

        private void ExecuteRequest(Action functionRequest)
        {
            ProcessRequest(() =>
            {
                functionRequest();
                return true;
            });
        }
    }
}