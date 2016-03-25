using System;
using DataGenericCache.Adapters;
using DataGenericCache.Settings;
using DataGenericCache.Settings.ConfigSections;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataGenericCache.Tests.Settings
{
    [TestFixture]
    public class ProviderSettingsConfigTest
    {
        private IProviderSettingsConfig _providerSettingsConfig;
        private Mock<IConfigurationAdapter> _mockConfigurationAdapter;

        [SetUp]
        public void SetUp()
        {
            _mockConfigurationAdapter = new Mock<IConfigurationAdapter>();
            _providerSettingsConfig = new ProviderSettingsConfig(_mockConfigurationAdapter.Object);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "failed to load settings providers structure from configuration file.")]
        public void WhenGetProviders_AndGetSectionsCallReturnNull_ShouldThrowAnException()
        {
            _mockConfigurationAdapter
                .Setup(it => it.GetSections<CacheSection>(It.IsAny<string>()))
                .Returns((CacheSection)null);

            _providerSettingsConfig.GetProviders();
        }

        [Test]
        public void WhenGetActiveProviderCacheInMinutes_ReturnIntegerMaxValue_WhenObtainedValueIsZero()
        {
            var cacheSection = new CacheSection
            {
                ActiveProviderCacheInMinutes = new CacheSectionActiveProviderCacheInMinutes { Value = 0 }
            };

            _mockConfigurationAdapter
                .Setup(it => it.GetSections<CacheSection>(It.IsAny<string>()))
                .Returns(cacheSection);

            _providerSettingsConfig.GetActiveProviderCacheInMinutes()
                .Should()
                .Be(int.MaxValue);
        }

        [Test]
        public void WhenGetActiveProviderCacheInMinutes_ReturnDefaultValue_WhenObtainedValueIsNull()
        {
            const int expectedDefaultValue = 120;

            _mockConfigurationAdapter
                .Setup(it => it.GetSections<CacheSection>(It.IsAny<string>()))
                .Returns(new CacheSection { ActiveProviderCacheInMinutes = null});

            _providerSettingsConfig.GetActiveProviderCacheInMinutes()
                .Should()
                .Be(expectedDefaultValue);
        }
    }
}