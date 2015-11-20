using System.Collections.Generic;

namespace Data.Generic.Cache.Settings
{
    public interface IProviderSettingsConfig
    {
        IEnumerable<ProviderSettings> GetProviders();

        int GetActiveProviderCacheInMinutes();
    }
}