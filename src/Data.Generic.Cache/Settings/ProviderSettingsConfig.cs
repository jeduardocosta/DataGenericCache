using System;
using System.Collections.Generic;
using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Settings.ConfigSections;

namespace Data.Generic.Cache.Settings
{
    public interface IProviderSettingsConfig
    {
        IEnumerable<ProviderSettings> GetProviders();

        int GetActiveProviderCacheInMinutes();
    }

    internal class ProviderSettingsConfig : IProviderSettingsConfig
    {
        private const string KeyName = "dataGenericCacheSection";

        private readonly IConfigurationAdapter _configurationAdapter;

        public ProviderSettingsConfig(IConfigurationAdapter configurationAdapter)
        {
            _configurationAdapter = configurationAdapter;
        }

        public IEnumerable<ProviderSettings> GetProviders()
        {
            var dataGenericCacheSection = _configurationAdapter.GetSections<CacheSection>(KeyName);

            if (dataGenericCacheSection == null)
                throw new Exception("failed to load settings providers structure from configuration file.");

            var providerSettings = new List<ProviderSettings>();
            
            foreach (CacheSectionProviderElement provider in dataGenericCacheSection.Providers)
                providerSettings.Add(new ProviderSettings(new ServerSettings(provider.Server, provider.Port, provider.Password), provider.Type));

            return providerSettings;
        }

        public int GetActiveProviderCacheInMinutes()
        {
            const int defaultValue = 120;
            var activeProviderCacheInMinutes = defaultValue;

            var dataGenericCacheSettings = _configurationAdapter.GetSections<CacheSection>(KeyName);

            if (HasActiveProviderCacheInMinutesValue(dataGenericCacheSettings))
                if (dataGenericCacheSettings.ActiveProviderCacheInMinutes.Value == 0)
                    activeProviderCacheInMinutes = int.MaxValue;
                else
                    activeProviderCacheInMinutes = dataGenericCacheSettings.ActiveProviderCacheInMinutes.Value;

            return activeProviderCacheInMinutes;
        }

        private bool HasActiveProviderCacheInMinutesValue(CacheSection dataGenericCacheSettings)
        {
            return dataGenericCacheSettings != null && 
                   dataGenericCacheSettings.ActiveProviderCacheInMinutes != null;
        }
    }
}