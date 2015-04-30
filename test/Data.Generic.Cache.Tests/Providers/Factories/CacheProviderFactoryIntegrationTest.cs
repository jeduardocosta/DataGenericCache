using Data.Generic.Cache.Providers.Factories;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers.Factories
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
        public void Should_GetCacheProvider_InCacheProviderFactory()
        {
            _cacheProviderFactory
                .Create()
                .Should()
                .NotBeNull();
        }
    }
}