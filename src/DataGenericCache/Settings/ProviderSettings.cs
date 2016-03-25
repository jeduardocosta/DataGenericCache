namespace DataGenericCache.Settings
{
    internal class ProviderSettings : IProviderSettings
    {
        public ServerSettings ServerSettings { get; }
        public CacheProvider Type { get; }

        public ProviderSettings(ServerSettings serverSettings, CacheProvider type)
        {
            ServerSettings = serverSettings;
            Type = type;
        }
    }
}