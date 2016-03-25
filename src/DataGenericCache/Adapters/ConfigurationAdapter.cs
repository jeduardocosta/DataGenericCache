using System.Configuration;

namespace DataGenericCache.Adapters
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