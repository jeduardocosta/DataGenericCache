using System;
using System.IO;
using System.Linq;
using Data.Generic.Cache.Extensions;
using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers
{
    [TestFixture]
    public class LocalStorageCacheProviderIntegrationTest
    {
        private ICacheProvider _localStorageCacheProvider;
        private ServerSettings _serverSettings;

        private const string BasePath = "c:\\data";

        private string _key;
        private string _expectedPath;

        [SetUp]
        public void SetUp()
        {
            _localStorageCacheProvider = new LocalStorageCacheProvider();

            _serverSettings = new ServerSettings(BasePath);
            _localStorageCacheProvider.SetupConfiguration(_serverSettings);

            _key = Guid.NewGuid().ToString();
            _expectedPath = BasePath.Combine(_key);

            _localStorageCacheProvider.Add(_key, "value", TimeSpan.FromSeconds(1));
        }

        [TearDown]
        public void TearDown()
        {
            var files = Directory.GetFiles(BasePath).Where(e => e.Contains(_key)).ToArray();
            Array.ForEach(files, File.Delete);
        }

        [Test]
        public void Add_GivenKeyAndValueAndExpiration_ShouldAddFile()
        {
            File.Exists(_expectedPath).Should().BeTrue();
        }

        [Test]
        public void Remove_GivenKey_ShouldRemoveFile()
        {
            _localStorageCacheProvider.Remove(_key);

            File.Exists(_expectedPath).Should().BeFalse();
        }

        [Test]
        public void Retrieve_GivenKey_ShouldReturnExpectedValue()
        {
            _localStorageCacheProvider
                .Retrieve<string>(_key)
                .Should()
                .Be("value");
        }
        
        [Test]
        public void RetrieveOrElse_GivenKeyThatNotExists_ShouldReturnExpectedValue()
        {
            var newKey = $"{_key}-new";

            _localStorageCacheProvider
                .RetrieveOrElse(newKey, TimeSpan.FromSeconds(1), () => "new value")
                .Should()
                .Be("new value");
        }
        
        [Test]
        public void IsWorking_GivenLocalStorageCacheProvider_ShouldReturnTrue()
        {
            _localStorageCacheProvider
                .IsWorking()
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void CreateFilePath_GivenKey_ShouldReturnExpectedValue()
        {
            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .CreateFilePath(_key)
                .Should()
                .Be(BasePath.Combine(_key));
        }
    }
}