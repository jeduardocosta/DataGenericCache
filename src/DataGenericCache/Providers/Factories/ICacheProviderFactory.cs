namespace DataGenericCache.Providers.Factories
{
    internal interface ICacheProviderFactory
    {
        ICacheProvider Create();

        int GetTotalAvailableProviders();
    }
}