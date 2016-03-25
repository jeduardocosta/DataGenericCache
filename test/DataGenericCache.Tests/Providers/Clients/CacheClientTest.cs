using System;
using System.Collections.Generic;
using DataGenericCache.Exceptions;
using DataGenericCache.Providers;
using DataGenericCache.Providers.Clients;
using DataGenericCache.Providers.Factories;
using DataGenericCache.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers.Clients
{
    [TestFixture]
    public class CacheClientTest
    {
        private ICacheProvider _cacheClient;

        private Mock<ICacheProviderFactory> _mockCacheProviderFactory;
        private Mock<ICacheProvider> _mockCacheProvider;

        private const string KeyName = "key";
        private readonly object _value = "123";
        private readonly TimeSpan _expiration = TimeSpan.FromMinutes(1);

        [SetUp]
        public void SetUp()
        {
            _mockCacheProviderFactory = new Mock<ICacheProviderFactory>();
            _mockCacheProvider = new Mock<ICacheProvider>();

            _mockCacheProviderFactory
                .Setup(it => it.Create())
                .Returns(_mockCacheProvider.Object);

            _mockCacheProviderFactory
                .Setup(it => it.GetTotalAvailableProviders())
                .Returns(It.IsAny<int>());

            _cacheClient = new CacheClient(_mockCacheProviderFactory.Object);
        }

        [Test]
        public void Should_AddValue_InCacheClient()
        {
            _cacheClient.Add(KeyName, _value, _expiration);

            _mockCacheProvider.Verify(it => it.Add(KeyName, _value, _expiration), Times.Once);
        }

        [Test]
        public void Should_SetValue_InCacheClient()
        {
            _cacheClient.Set(KeyName, _value, _expiration);

            _mockCacheProvider.Verify(it => it.Set(KeyName, _value, _expiration), Times.Once);
        }

        [Test]
        public void Should_RemoveValue_InCacheClient()
        {
            _cacheClient.Remove(KeyName);

            _mockCacheProvider.Verify(it => it.Remove(KeyName), Times.Once);
        }

        [Test]
        public void Should_CheckIfExists_InCacheClient()
        {
            _cacheClient.Exists(KeyName);

            _mockCacheProvider.Verify(it => it.Exists(KeyName), Times.Once);
        }

        [Test]
        public void Should_RetrieveValue_InCacheClient()
        {
            _cacheClient.Retrieve<string>(KeyName);

            _mockCacheProvider.Verify(it => it. Retrieve<string>(KeyName), Times.Once);
        }

        [Test]
        public void Should_RetrieveValueOrCallFunction_InCacheClient()
        {
            Func<string> callback = () => "";

            _cacheClient.RetrieveOrElse(KeyName, _expiration, callback);

            _mockCacheProvider.Verify(it => it.RetrieveOrElse(KeyName, _expiration, callback), Times.Once);
        }

        [Test]
        public void Should_SetupConfiguration_ToCacheProvider_InCacheClient()
        {
            var serverSettings = It.IsAny<ServerSettings>();

            _cacheClient.SetupConfiguration(serverSettings);

            _mockCacheProvider.Verify(it => it.SetupConfiguration(serverSettings), Times.Once);
        }

        [Test]
        public void WhenProcessRequest_WithCacheClient_ShouldNotThrowAnException_WhenAnErrorOccurred()
        {
            _mockCacheProvider
                .Setup(it => it.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
                .Throws(new Exception());

            _cacheClient.Add("key", "value", new TimeSpan());
        }

        [Test]
        public void WhenProcessRequest_WithCacheClient_ShouldGetTypeDefaultValue_ToPrimitiveType_WhenAnErrorOccurred()
        {
            _mockCacheProvider
                .Setup(it => it.Retrieve<bool>(It.IsAny<string>()))
                .Throws(new Exception());

            _cacheClient.Retrieve<bool>("key")
                .Should()
                .Be(false);
        }

        [Test]
        public void WhenProcessRequest_WithCacheClient_ShouldGetTypeDefaultValue_WhenAnErrorOccurred()
        {
            _mockCacheProvider
                .Setup(it => it.Retrieve<IEnumerable<ServerSettings>>(It.IsAny<string>()))
                .Throws(new Exception());

            _cacheClient.Retrieve<IEnumerable<ServerSettings>>("key")
                .Should()
                .BeNull();
        }

        [Test]
        public void WhenProcessRequest_ShouldGetTotalAvailableProviders_WhenAnCacheProviderConnectionExceptionIsThrow()
        {
            _mockCacheProvider
                .Setup(it => it.Retrieve<IEnumerable<ServerSettings>>(It.IsAny<string>()))
                .Throws(new CacheProviderConnectionException());

            _cacheClient.Retrieve<IEnumerable<ServerSettings>>("key");

            _mockCacheProviderFactory.Verify(it => it.GetTotalAvailableProviders(), Times.Once);
        }

        [Test]
        public void WhenProcessRequest_ShouldNotGetTotalAvailableProviders_WhenAnExceptionIsThrow()
        {
            _mockCacheProvider
                .Setup(it => it.Retrieve<IEnumerable<ServerSettings>>(It.IsAny<string>()))
                .Throws(new Exception());

            _cacheClient.Retrieve<IEnumerable<ServerSettings>>("key");

            _mockCacheProviderFactory.Verify(it => it.GetTotalAvailableProviders(), Times.Never);
        }
    }
}