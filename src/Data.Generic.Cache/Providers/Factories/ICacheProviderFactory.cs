namespace Data.Generic.Cache.Providers.Factories
{
    public interface ICacheProviderFactory
    {
        ICacheProvider Create();

        int GetTotalAvailableProviders();
    }
}