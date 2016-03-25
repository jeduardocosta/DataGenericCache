using DataGenericCache.Settings;

namespace DataGenericCache.Providers.Factories
{
    internal interface ICacheProviderInstanceFactory
    {
        ICacheProvider Create(CacheProvider cacheProviderType, ServerSettings serverSettings);
    }
}