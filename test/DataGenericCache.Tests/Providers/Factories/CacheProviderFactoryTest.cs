using System;
using System.Linq;
using DataGenericCache.Providers;
using DataGenericCache.Providers.Factories;
using DataGenericCache.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers.Factories
{
    [TestFixture]
    public class CacheProviderFactoryTest
    {
        private ICacheProviderFactory _cacheProviderFactory;

        private Mock<ICacheProvider> _mockLocalMemoryProvider;
        private Mock<IProviderSettingsConfig> _mockProviderSettingsConfig;
        private Mock<ICacheProviderInstanceFactory> _mockCacheProviderInstanceFactory;
        private Mock<ICacheProvider> _mockRedisCacheProvider;

        private static readonly ServerSettings ServerSettings = new ServerSettings();
        private readonly ProviderSettings _providerSettings = new ProviderSettings(ServerSettings, CacheProvider.Redis);
        private const string ActiveCacheProviderKeyName = "CacheProviderFactory.GetActiveProviderCache.ActiveCache";

        [SetUp]
        public void SetUp()
        {
            _mockLocalMemoryProvider = new Mock<ICacheProvider>();
            _mockProviderSettingsConfig = new Mock<IProviderSettingsConfig>();
            _mockCacheProviderInstanceFactory = new Mock<ICacheProviderInstanceFactory>();

            _mockProviderSettingsConfig.Setup(e => e.GetProviders()).Returns(new[] {_providerSettings});

            _mockRedisCacheProvider = new Mock<ICacheProvider>();
            _mockRedisCacheProvider.Setup(e => e.Retrieve<string>(It.IsAny<string>())).Returns("isWorking");

            _mockCacheProviderInstanceFactory.Setup(e => e.Create(CacheProvider.Redis, ServerSettings)).Returns(_mockRedisCacheProvider.Object);

            _mockLocalMemoryProvider.Setup(e => e.Exists(ActiveCacheProviderKeyName)).Returns(true);
            _mockLocalMemoryProvider.Setup(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName)).Returns(_mockRedisCacheProvider.Object);

            _cacheProviderFactory = new CacheProviderFactory(_mockLocalMemoryProvider.Object,
                _mockProviderSettingsConfig.Object,
                _mockCacheProviderInstanceFactory.Object);
        }

        [Test]
        public void Create_GivenCacheProviderFactory_ShouldCallExistsOnLocalMemoryProvider()
        {
            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(e => e.Exists(ActiveCacheProviderKeyName), Times.Once);
        }

        [Test]
        public void Create_GivenCacheProviderFactory_ShouldCallRetrieveOnLocalMemoryProvider()
        {
            _cacheProviderFactory.Create();

            _mockLocalMemoryProvider.Verify(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName), Times.Once);
        }

        [Test]

        public void GetTotalAvailableProviders_GivenCacheProviderFactory_ShouldCallGetProvidersOnProviderSettingsConfig()
        {
            _cacheProviderFactory.GetTotalAvailableProviders();

            _mockProviderSettingsConfig.Verify(e => e.GetProviders(), Times.Once);
        }

        [Test]

        public void GetTotalAvailableProviders_GivenCacheProviderFactory_ShouldReturnExpectedTotalProviders()
        {
            _mockProviderSettingsConfig.Setup(e => e.GetProviders()).Returns(Enumerable.Repeat(_providerSettings, 3));

            _cacheProviderFactory
                .GetTotalAvailableProviders()
                .Should()
                .Be(3);
        }

        [Test]
        public void GetTotalAvailableProviders_WhenThrowException_ShouldBe1()
        {
            _mockProviderSettingsConfig.Setup(e => e.GetProviders()).Throws(new Exception());

            _cacheProviderFactory
                .GetTotalAvailableProviders()
                .Should()
                .Be(1);
        }

        [Test]
        public void Create_WhenActiveCacheProviderIsNull_ShouldCallGetProvidersOnProviderSettingsConfig()
        {
            _mockLocalMemoryProvider.Setup(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName)).Returns((ICacheProvider)null);

            _cacheProviderFactory.Create();

            _mockProviderSettingsConfig.Verify(e => e.GetProviders(), Times.Once);
        }

        [Test]
        public void Create_WhenGettingCacheProvider_ShouldCallAddInWorkingTest()
        {
            _mockLocalMemoryProvider.Setup(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName)).Returns((ICacheProvider)null);

            _cacheProviderFactory.Create();

            _mockRedisCacheProvider.Verify(e => e.Add(It.IsAny<string>(), "isWorking", It.IsAny<TimeSpan>()), Times.Once);
        }

        [Test]
        public void Create_WhenGettingCacheProvider_ShouldCallRemoveInWorkingTest()
        {
            _mockLocalMemoryProvider.Setup(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName)).Returns((ICacheProvider)null);

            _cacheProviderFactory.Create();

            _mockRedisCacheProvider.Verify(e => e.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Create_WhenThrowExceptionInWorkingTest_ShouldReturnExpectedException()
        {
            _mockLocalMemoryProvider.Setup(e => e.Retrieve<ICacheProvider>(ActiveCacheProviderKeyName)).Returns((ICacheProvider)null);
            _mockRedisCacheProvider.Setup(e => e.Remove(It.IsAny<string>())).Throws(new Exception());

            Action action = () => _cacheProviderFactory.Create();

            action
                .ShouldThrow<Exception>()
                .WithMessage("failed to get cache provider.");
        }
    }
}