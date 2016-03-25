using DataGenericCache.Providers;
using DataGenericCache.Providers.Factories;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers.Factories
{
    [TestFixture]
    public class CacheProviderFactoryIntegrationTest
    {
        private ICacheProviderFactory _cacheProviderFactory;

        [SetUp]
        public void SetUp()
        {
            _cacheProviderFactory = new CacheProviderFactory();
        }

        [Test]
        public void Create_GivenAppSettings_ShouldReturnFirstSetProvider()
        {
            _cacheProviderFactory
                .Create()
                .Should()
                .BeOfType<RedisCacheProvider>();
        }
    }
}