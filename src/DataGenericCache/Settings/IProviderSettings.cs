namespace DataGenericCache.Settings
{
    public interface IProviderSettings
    {
        ServerSettings ServerSettings { get; }
        CacheProvider Type { get; }
    }
}