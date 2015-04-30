using System.Configuration;

namespace Data.Generic.Cache.Settings.ConfigSections
{
    public class CacheSection : ConfigurationSection
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