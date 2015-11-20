using System;
using System.IO;
using System.Linq;
using System.Threading;
using Data.Generic.Cache.Extensions;
using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers
{
    [TestFixture]
    public class LocalStorageCacheProviderTimeSpanIntegrationTest
    {
        private ICacheProvider _localStorageCacheProvider;
        private ServerSettings _serverSettings;

        private const string BasePath = "c:\\data";
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _localStorageCacheProvider = new LocalStorageCacheProvider();

            _serverSettings = new ServerSettings(BasePath);
            _localStorageCacheProvider.SetupConfiguration(_serverSettings);

            _key = Guid.NewGuid().ToString();

            _localStorageCacheProvider.Add(_key, "value", TimeSpan.FromSeconds(1));
        }

        [TearDown]
        public void TearDown()
        {
            var files = Directory.GetFiles(BasePath).Where(e => e.Contains(_key)).ToArray();
            Array.ForEach(files, File.Delete);
        }

        [Test]
        public void Add_GivenKeyAndValueAndTimeExpiration_ShouldAddExpirationFile()
        {
            var expectedPath = BasePath.Combine($"{_key}-expiration");

            File.Exists(expectedPath).Should().BeTrue();
        }

        [Test]
        public void Retrieve_GivenExpiratedTime_ShouldBeNull()
        {
            Thread.Sleep(1000);

            _localStorageCacheProvider
                .Retrieve<string>(_key)
                .Should()
                .BeNull();
        }
    }
}