namespace DataGenericCache.Providers.Factories
{
    public interface ICacheProviderFactory
    {
        ICacheProvider Create();

        int GetTotalAvailableProviders();
    }
}