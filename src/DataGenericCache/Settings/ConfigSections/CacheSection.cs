using System.Configuration;

namespace DataGenericCache.Settings.ConfigSections
{
    internal class CacheSection : ConfigurationSection
    {
        [ConfigurationProperty("providers")]
        public CacheSectionProviderElementCollection Providers
        {
            get { return (CacheSectionProviderElementCollection)base["providers"]; }
            set { base["providers"] = value; }
        }

        [ConfigurationProperty("activeProviderCacheInMinutes")]
        public CacheSectionActiveProviderCacheInMinutes ActiveProviderCacheInMinutes
        {
            get { return (CacheSectionActiveProviderCacheInMinutes)base["activeProviderCacheInMinutes"]; }
            set { base["activeProviderCacheInMinutes"] = value; }
        }
    }
}