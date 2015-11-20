using System;
using System.Collections.Generic;
using System.Linq;
using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Settings;

namespace Data.Generic.Cache.Providers.Factories
{
    internal class CacheProviderFactory : ICacheProviderFactory
    {
        private readonly ICacheProvider _localMemoryProvider;
        private readonly IProviderSettingsConfig _providerSettingsConfig;
        private readonly ICacheProviderInstanceFactory _cacheProviderInstanceFactory;

        public CacheProviderFactory()
        {
            _localMemoryProvider = new LocalMemoryCacheProvider();
            _providerSettingsConfig = new ProviderSettingsConfig(new ConfigurationAdapter());
            _cacheProviderInstanceFactory = new CacheProviderInstanceFactory();
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
            var providerSettings = _providerSettingsConfig.GetProviders();
            var cacheProviders = providerSettings.Select(it => _cacheProviderInstanceFactory.Create(it.Type, it.ServerSettings)).ToList();
            var activeCacheProvider = GetActiveCache(cacheProviders);
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

        private ICacheProvider GetActiveCache(IEnumerable<ICacheProvider> cacheProviders)
        {
            const string testContentKeyName = "CacheProviderFactory.GetActiveCache.ActiveCache";

            foreach (var cacheProvider in cacheProviders)
            {
                try
                {
                    ICacheProvider activeCacheProvider;

                    if (_localMemoryProvider.Exists(testContentKeyName))
                    {
                        activeCacheProvider = _localMemoryProvider.Retrieve<ICacheProvider>(testContentKeyName);
                    }
                    else
                    {
                        if (!IsWorking(cacheProvider))
                        { 
                            continue;
                        }

                        var activeProviderCacheInMinutes = _providerSettingsConfig.GetActiveProviderCacheInMinutes();
                        _localMemoryProvider.Add(testContentKeyName, cacheProvider, TimeSpan.FromMinutes(activeProviderCacheInMinutes));

                        activeCacheProvider = cacheProvider;
                    }

                    return activeCacheProvider;
                }
                catch (Exception)
                { }
            }

            throw new Exception("failed to get cache provider.");
        }


        private static bool IsWorking(ICacheProvider cacheProvider)
        {
            const string value = "isworking";
            var key = Guid.NewGuid().ToString();
            var isWorking = false;

            try
            {
                cacheProvider.Add(key, value, TimeSpan.FromSeconds(2));
                isWorking = value == cacheProvider.Retrieve<string>(key);
                cacheProvider.Remove(key);
            }
            catch (Exception)
            { }

            return isWorking;
        }
    }
}