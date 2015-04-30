using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Settings
{
    [TestFixture]
    public class ProviderSettingsConfigIntegrationTest
    {
        private IProviderSettingsConfig _providerSettingsConfig;

        [SetUp]
        public void SetUp()
        {
            _providerSettingsConfig = new ProviderSettingsConfig(new ConfigurationAdapter());
        }

        [Test]
        public void Should_GetProviders_FromConfigurationFile()
        {
            _providerSettingsConfig
                .GetProviders()
                .Should()
                .NotBeNull();
        }

        [Test]
        public void Should_GetActiveProviderCacheInMinutes_FromConfigurationFile()
        {
            _providerSettingsConfig
                .GetActiveProviderCacheInMinutes()
                .Should()
                .BeGreaterThan(0);
        }
    }
}