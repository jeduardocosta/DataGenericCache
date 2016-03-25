using System.Collections.Generic;

namespace DataGenericCache.Settings
{
    internal interface IProviderSettingsConfig
    {
        IEnumerable<ProviderSettings> GetProviders();

        int GetActiveProviderCacheInMinutes();
    }
}