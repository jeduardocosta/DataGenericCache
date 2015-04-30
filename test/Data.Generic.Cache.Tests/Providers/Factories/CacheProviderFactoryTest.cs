using System;
using System.Collections.Generic;
using System.Linq;
using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Providers.Factories;
using Data.Generic.Cache.Settings;
using Moq;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers.Factories
{
    [TestFixture]
    public class CacheProviderFactoryTest
    {
        private ICacheProviderFactory _cacheProviderFactory;

        private Mock<ICacheProvider> _mockLocalMemoryProvider;
        private Mock<IProviderSettingsConfig> _mockProviderSettingsConfig;
        private Mock<ICacheProviderInstanceFactory> _mockCacheProviderInstanceFactory;

        private IEnumerable<ProviderSettings> _providerSettings;

        private Mock<ICacheProvider> _cacheProvider;
        private Mock<ICacheProvider> _redisCacheProvider;

        private const string ActiveCacheKeyName = "CacheProviderFactory.GetActiveCache.ActiveCache";

        [SetUp]
        public void SetUp()
        {
            _mockLocalMemoryProvider = new Mock<ICacheProvider>();
            _mockProviderSettingsConfig = new Mock<IProviderSettingsConfig>();
            _mockCacheProviderInstanceFactory = new Mock<ICacheProviderInstanceFactory>();

            _providerSettings = GivenAProviderSettings();

            _cacheProvider = new Mock<ICacheProvider>();
            _redisCacheProvider = new Mock<ICacheProvider>();

            _cacheProvider.Setup(it => it.IsWorking()).Returns(true);
            _redisCacheProvider.Setup(it => it.IsWorking()).Returns(true);

            _mockProviderSettingsConfig
                .Setup(it => it.GetProviders())
                .Returns(_providerSettings);

            SetUpMockCacheProviderInstanceFactory();

            _cacheProviderFactory = new CacheProviderFactory(_mockLocalMemoryProvider.Object,
                _mockProviderSettingsConfig.Object,
                _mockCacheProviderInstanceFactory.Object);
        }

        [Test]
        public void GetProviders_InCacheProviderFactory()
        {
            _cacheProviderFactory.Create();

            _mockProviderSettingsConfig.Verify(it => it.GetProviders(), Times.Once);
        }

        [Test]
        public void CreateInstances_InCacheProviderFactory()
        {
            _cacheProviderFactory.Create();

            var expectedTimes = _providerSettings.Count();

            _mockCacheProviderInstanceFactory
                .Verify(it => it.Create(It.IsAny<CacheProvider>(), It.IsAny<ServerSettings>()), Times.Exactly(expectedTimes));
        }

        [Test]
        public void Should_CreateCacheProvider_AndCallExistsMethod_WhenGettingActiveCache_InCacheProviderFactory()
        {
            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(it => it.Exists(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UseSpecificKeyName_InExistsMethod_WhenGettingActiveCache_InCacheProviderFactory()
        {
            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(it => it.Exists(ActiveCacheKeyName), Times.Once);
        }

        [Test]
        public void RetrieveCacheProvider_WhenExistsValue_ForGettingActiveCacheProcess_InCacheProviderFactory()
        {
            SetupExistsMethodInLocalMemoryProviderWith(true);

            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(it => it.Retrieve<ICacheProvider>(ActiveCacheKeyName), Times.Once);
        }

        [Test]
        public void ValidateCacheProvider_WhenCacheProviderThatNotExistsInLocalMemory_InCacheProviderFactory()
        {
            SetupExistsMethodInLocalMemoryProviderWith(false);

            _cacheProviderFactory.Create();

            _cacheProvider.Verify(it => it.IsWorking(), Times.Once);
        }

        [Test]
        public void Should_GetActiveProviderCacheInMinutes_WhenSetACtiveProviderCache_InCacheProviderFactory()
        {
            SetupExistsMethodInLocalMemoryProviderWith(false);

            _cacheProviderFactory.Create();

            _mockProviderSettingsConfig.Verify(it => it.GetActiveProviderCacheInMinutes(), Times.Once);
        }

        [Test]
        public void AddCacheProvider_ToLocalMemoryProvider_InCacheProviderFactory()
        {
            SetupExistsMethodInLocalMemoryProviderWith(false);

            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(it => it.Add(ActiveCacheKeyName, It.IsAny<ICacheProvider>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Test]
        public void UseActiveProviderCacheInMinutes_FromProviderSettings_WhenAddingCacheProvider_ToLocalMemoryProvider()
        {
            const int cacheInMinutes = 10;

            var expectedTimeSpan = TimeSpan.FromMinutes(cacheInMinutes);

            _mockProviderSettingsConfig
                .Setup(it => it.GetActiveProviderCacheInMinutes())
                .Returns(cacheInMinutes);

            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(it => it.Add(ActiveCacheKeyName, It.IsAny<ICacheProvider>(), expectedTimeSpan), Times.Once);
        }

        [Test]
        public void GetRedisCacheProvider_WhenLocalMemoryCacheProviderThrownAnException_InCacheProviderFactory()
        {
            _cacheProvider.Setup(it => it.IsWorking()).Returns(false);

            _cacheProviderFactory.Create();

            _cacheProvider.Verify(it => it.IsWorking(), Times.Once);
            _redisCacheProvider.Verify(it => it.IsWorking(), Times.Once);
        }

        private IEnumerable<ProviderSettings> GivenAProviderSettings()
        {
            return new List<ProviderSettings>
            {
                new ProviderSettings(new ServerSettings("", 0, ""), CacheProvider.LocalMemory),
                new ProviderSettings(new ServerSettings("", 0, ""), CacheProvider.Redis)
            };
        }

        private void SetupExistsMethodInLocalMemoryProviderWith(bool value)
        {
            _mockLocalMemoryProvider
                .Setup(it => it.Exists(It.IsAny<string>()))
                .Returns(value);
        }

        private void SetUpMockCacheProviderInstanceFactory()
        {
            _mockCacheProviderInstanceFactory
                .Setup(it => it.Create(CacheProvider.LocalMemory, It.IsAny<ServerSettings>()))
                .Returns(_cacheProvider.Object);

            _mockCacheProviderInstanceFactory
                .Setup(it => it.Create(CacheProvider.Redis, It.IsAny<ServerSettings>()))
                .Returns(_redisCacheProvider.Object);
        }
    }
}