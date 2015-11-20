using Data.Generic.Cache.Settings;

namespace Data.Generic.Cache.Providers.Factories
{
    public interface ICacheProviderInstanceFactory
    {
        ICacheProvider Create(CacheProvider cacheProviderType, ServerSettings serverSettings);
    }
}