using DataGenericCache.Providers;
using DataGenericCache.Providers.Factories;
using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers.Factories
{
    [TestFixture]
    public class CacheProviderInstanceFactoryTest
    {
        private ICacheProviderInstanceFactory _cacheProviderInstanceFactory;
        private ServerSettings _serverSettings;

        [SetUp]
        public void Setup()
        {
            _cacheProviderInstanceFactory = new CacheProviderInstanceFactory();
            _serverSettings = new ServerSettings("", 0, "");
        }

        [Test]
        public void Should_GetRedisCacheProviderInstance_ByProviderType()
        {
            _cacheProviderInstanceFactory.Create(CacheProvider.Redis, _serverSettings)
                .Should()
                .BeOfType<RedisCacheProvider>();
        }

        [Test]
        public void Should_GetLocalMemoryProviderInstance_ByProviderType()
        {
            _cacheProviderInstanceFactory.Create(CacheProvider.LocalMemory, _serverSettings)
                .Should()
                .BeOfType<LocalMemoryCacheProvider>();
        }

        [Test]
        public void Should_GetLocalStorageProviderInstance_ByProviderType()
        {
            _cacheProviderInstanceFactory.Create(CacheProvider.LocalStorage, _serverSettings)
                .Should()
                .BeOfType<LocalStorageCacheProvider>();
        }
    }
}