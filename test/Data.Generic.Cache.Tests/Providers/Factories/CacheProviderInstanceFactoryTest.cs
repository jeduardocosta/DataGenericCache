﻿using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Providers.Factories;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers.Factories
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
            _serverSettings = GivenAServerSettings();
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

        private ServerSettings GivenAServerSettings()
        {
            return new ServerSettings("", 0, "");
        }
    }
}