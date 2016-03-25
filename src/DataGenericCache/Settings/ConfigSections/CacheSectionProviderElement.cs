using System.ComponentModel;
using System.Configuration;

namespace DataGenericCache.Settings.ConfigSections
{
    public class CacheSectionProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("type")]
        [TypeConverter(typeof(CaseInsensitiveEnumConfigConverter<CacheProvider>))]
        public CacheProvider Type
        {
            get { return (CacheProvider) this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("address")]
        public string Address
        {
            get { return this["address"] as string; }
            set { this["address"] = value; }
        }

        [ConfigurationProperty("port")]
        public int Port
        {
            get { return (int) this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return this["password"] as string; }
            set { this["password"] = value; }
        }

        public string Id => $"{Type}-{Address}-{Port}";
    }
}