using System;
using System.Collections.Generic;
using Data.Generic.Cache.Settings;

namespace Data.Generic.Cache.Providers.Factories
{
    public interface ICacheProviderInstanceFactory
    {
        ICacheProvider Create(CacheProvider cacheProviderType, ServerSettings serverSettings);
    }

    internal class CacheProviderInstanceFactory : ICacheProviderInstanceFactory
    {
        private static readonly Dictionary<CacheProvider, Func<ICacheProvider>> Instances
            = new Dictionary<CacheProvider, Func<ICacheProvider>>
            {
                {CacheProvider.Redis, () => new RedisCacheProvider()},
                {CacheProvider.LocalMemory, () => new LocalMemoryCacheProvider()}
            };

        public ICacheProvider Create(CacheProvider cacheProviderType, ServerSettings serverSettings)
        {
            var cacheProvider = Instances[cacheProviderType]();
            cacheProvider.SetupConfiguration(serverSettings);
            return cacheProvider;
        }
    }
}