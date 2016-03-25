using DataGenericCache.Settings;

namespace DataGenericCache.Providers.Factories
{
    public interface ICacheProviderInstanceFactory
    {
        ICacheProvider Create(CacheProvider cacheProviderType, ServerSettings serverSettings);
    }
}