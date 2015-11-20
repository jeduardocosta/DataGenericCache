using System.Configuration;

namespace Data.Generic.Cache.Adapters
{
    internal class ConfigurationAdapter : IConfigurationAdapter
    {
        public string Get(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }

        public T GetSections<T>(string keyName)
        {
            return (T)ConfigurationManager.GetSection(keyName);
        }
    }
}