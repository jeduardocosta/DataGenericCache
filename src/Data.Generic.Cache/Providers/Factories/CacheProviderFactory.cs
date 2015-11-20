using System;
using System.Collections.Generic;
using System.Linq;
using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Settings;

namespace Data.Generic.Cache.Providers.Factories
{
    internal class CacheProviderFactory : ICacheProviderFactory
    {
        private const string ActiveCacheProviderKeyName = "CacheProviderFactory.GetActiveProviderCache.ActiveCache";

        private readonly ICacheProvider _localMemoryProvider;
        private readonly IProviderSettingsConfig _providerSettingsConfig;
        private readonly ICacheProviderInstanceFactory _cacheProviderInstanceFactory;

        public CacheProviderFactory()
            : this(new LocalMemoryCacheProvider(), 
                  new ProviderSettingsConfig(new ConfigurationAdapter()), 
                  new CacheProviderInstanceFactory())
        {
        }

        public CacheProviderFactory(ICacheProvider localMemoryProvider, 
            IProviderSettingsConfig providerSettingsConfig, 
            ICacheProviderInstanceFactory cacheProviderInstanceFactory)
        {
            _localMemoryProvider = localMemoryProvider;
            _providerSettingsConfig = providerSettingsConfig;
            _cacheProviderInstanceFactory = cacheProviderInstanceFactory;
        }
        
        public ICacheProvider Create()
        {
            var activeCacheProvider = GetActiveCacheProvider();

            if (activeCacheProvider == null)
            {
                var providerSettings = _providerSettingsConfig.GetProviders();
                var cacheProviders = providerSettings.Select(it => _cacheProviderInstanceFactory.Create(it.Type, it.ServerSettings)).ToList();
                activeCacheProvider = GetCacheProvider(cacheProviders);
            }

            return activeCacheProvider;
        }

        public int GetTotalAvailableProviders()
        {
            int totalProviders;

            try
            {
                totalProviders = _providerSettingsConfig.GetProviders().Count();
            }
            catch (Exception)
            {
                totalProviders = 1;
            }

            return totalProviders;
        }

        private ICacheProvider GetActiveCacheProvider()
        {
            ICacheProvider activeCacheProvider = null;

            if (_localMemoryProvider.Exists(ActiveCacheProviderKeyName))
            {
                activeCacheProvider = _localMemoryProvider.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName);
            }

            return activeCacheProvider;
        }

        private ICacheProvider GetCacheProvider(IEnumerable<ICacheProvider> cacheProviders)
        {
            foreach (var cacheProvider in cacheProviders)
            {
                try
                {
                    if (!IsWorking(cacheProvider))
                    {
                        continue;
                    }

                    var activeProviderCacheInMinutes = _providerSettingsConfig.GetActiveProviderCacheInMinutes();
                    _localMemoryProvider.Add(ActiveCacheProviderKeyName, cacheProvider, TimeSpan.FromMinutes(activeProviderCacheInMinutes));

                    return cacheProvider;
                }
                catch (Exception)
                { }
            }

            throw new Exception("failed to get cache provider.");
        }


        private static bool IsWorking(ICacheProvider cacheProvider)
        {
            const string value = "isWorking";
            var key = Guid.NewGuid().ToString();
            bool isWorking;

            try
            {
                cacheProvider.Add(key, value, TimeSpan.FromSeconds(2));
                isWorking = value == cacheProvider.Retrieve<string>(key);
                cacheProvider.Remove(key);
            }
            catch (Exception)
            {
                isWorking = false;
            }

            return isWorking;
        }
    }
}