using System.Collections.Generic;

namespace DataGenericCache.Settings
{
    public interface IProviderSettingsConfig
    {
        IEnumerable<ProviderSettings> GetProviders();

        int GetActiveProviderCacheInMinutes();
    }
}