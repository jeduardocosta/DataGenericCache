using System.Configuration;

namespace Data.Generic.Cache.Settings.ConfigSections
{
    public class CacheSectionActiveProviderCacheInMinutes : ConfigurationElement
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