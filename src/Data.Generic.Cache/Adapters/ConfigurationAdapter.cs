using System.Configuration;

namespace Data.Generic.Cache.Adapters
{
    public interface IConfigurationAdapter
    {
        string Get(string keyName);

        T GetSections<T>(string keyName);
    }

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