using System.Configuration;

namespace DataGenericCache.Settings.ConfigSections
{
    internal class CacheSectionActiveProviderCacheInMinutes : ConfigurationElement
    {
        public const int Unlimited = 0;

        [ConfigurationProperty("value", DefaultValue = Unlimited)]
        public int Value
        {
            get { return (int)this["value"]; }
            set { this["value"] = value; }
        }
    }
}