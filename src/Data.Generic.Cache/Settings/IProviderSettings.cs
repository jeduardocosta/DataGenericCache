namespace Data.Generic.Cache.Settings
{
    public interface IProviderSettings
    {
        ServerSettings ServerSettings { get; }
        CacheProvider Type { get; }
    }
}