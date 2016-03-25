namespace DataGenericCache.Settings
{
    internal interface IProviderSettings
    {
        ServerSettings ServerSettings { get; }
        CacheProvider Type { get; }
    }
}