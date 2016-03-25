using System;
using System.Collections.Generic;
using DataGenericCache.Adapters;
using DataGenericCache.Settings.ConfigSections;

namespace DataGenericCache.Settings
{
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
            { 
                throw new Exception("failed to load settings providers structure from configuration file.");
            }

            var providerSettings = new List<ProviderSettings>();
            
            foreach (CacheSectionProviderElement provider in dataGenericCacheSection.Providers)
            { 
                providerSettings.Add(new ProviderSettings(new ServerSettings(provider.Address, provider.Port, provider.Password), provider.Type));
            }

            return providerSettings;
        }

        public int GetActiveProviderCacheInMinutes()
        {
            const int defaultValue = 120;
            var activeProviderCacheInMinutes = defaultValue;

            var dataGenericCacheSettings = _configurationAdapter.GetSections<CacheSection>(KeyName);

            if (HasActiveProviderCacheInMinutesValue(dataGenericCacheSettings))
            { 
                activeProviderCacheInMinutes = dataGenericCacheSettings.ActiveProviderCacheInMinutes.Value == 0 ? 
                    int.MaxValue : 
                    dataGenericCacheSettings.ActiveProviderCacheInMinutes.Value;
            }

            return activeProviderCacheInMinutes;
        }

        private bool HasActiveProviderCacheInMinutesValue(CacheSection dataGenericCacheSettings)
        {
            return dataGenericCacheSettings?.ActiveProviderCacheInMinutes != null;
        }
    }
}