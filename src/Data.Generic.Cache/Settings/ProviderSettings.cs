namespace Data.Generic.Cache.Settings
{
    public interface IProviderSettings
    {
        ServerSettings ServerSettings { get; }
        CacheProvider Type { get; }
    }

    public class ProviderSettings : IProviderSettings
    {
        public ServerSettings ServerSettings { get; private set; }
        public CacheProvider Type { get; private set; }

        public ProviderSettings(ServerSettings serverSettings, CacheProvider type)
        {
            ServerSettings = serverSettings;
            Type = type;
        }
    }
}