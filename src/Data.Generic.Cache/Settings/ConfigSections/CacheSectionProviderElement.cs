using System.ComponentModel;
using System.Configuration;

namespace Data.Generic.Cache.Settings.ConfigSections
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

        [ConfigurationProperty("server")]
        public string Server
        {
            get { return this["server"] as string; }
            set { this["server"] = value; }
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

        public string Id
        {
            get { return string.Format("{0}-{1}-{2}", Type, Server, Port); }
        }
    }
}