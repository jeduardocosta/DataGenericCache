using System;
using System.Globalization;
using DataGenericCache.Adapters;
using DataGenericCache.Extensions;
using DataGenericCache.Providers;
using DataGenericCache.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers
{
    [TestFixture]
    public class LocalStorageCacheProviderUnitTest
    {
        private Mock<IFileIoAdapter> _mockIoAdapter;
        private ICacheProvider _localStorageCacheProvider;
        private ServerSettings _serverSettings;

        private const string BasePath = "c:\\data";

        private string _key;
        private string _expectedPath;

        [SetUp]
        public void SetUp()
        {
            _mockIoAdapter = new Mock<IFileIoAdapter>();
            _mockIoAdapter.Setup(e => e.Retrieve(It.IsAny<string>())).Returns("");

            _localStorageCacheProvider = new LocalStorageCacheProvider(_mockIoAdapter.Object);

            _serverSettings = new ServerSettings(BasePath);
            _localStorageCacheProvider.SetupConfiguration(_serverSettings);

            _key = Guid.NewGuid().ToString();
            _expectedPath = BasePath.Combine(_key);

            Clock.Now = () => new DateTime(2015, 1, 1, 12, 30, 0);
        }

        [TearDown]
        public void TearDown()
        {
            Clock.ResetClock();
        }

        [Test]
        public void SetupConfiguration_GivenServerSettings_ShouldSetAddress()
        {
            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .Address
                .Should()
                .Be(BasePath);
        }

        [Test]
        public void Add_GivenKeyAndValueAndExpiration_ShouldCallAddOnIoAdapter()
        {
            var expectedValue = "value".ToJson();
            
            _localStorageCacheProvider.Add(_key, "value", default(TimeSpan));

            _mockIoAdapter.Verify(e => e.Add(BasePath, _key, expectedValue), Times.Once);
        }

        [Test]
        public void Add_GivenKeyAndValueAndExpiration_ShouldCallAddWithTimeExpirationOnIoAdapter()
        {
            var expiration = TimeSpan.FromMinutes(1);

            var expectedValue = new DateTime(2015, 1, 1, 12, 30, 0)
                .AddMinutes(1)
                .ToString(CultureInfo.InvariantCulture);

            _localStorageCacheProvider.Add(_key, "value", expiration);

            _mockIoAdapter.Verify(e => e.Add(BasePath, $"{_key}-expiration", expectedValue), Times.Once);
        }

        [Test]
        public void Remove_GivenKey_ShouldCallRemoveOnIoAdapter()
        {
            var expectedPath = BasePath.Combine($"{_key}-expiration");

            _localStorageCacheProvider.Remove(_key);

            _mockIoAdapter.Verify(e => e.Remove(expectedPath), Times.Once);
        }

        [Test]
        public void Remove_GivenKey_ShouldCallRemoveExpirationFileOnIoAdapter()
        {
            _localStorageCacheProvider.Remove(_key);

            _mockIoAdapter.Verify(e => e.Remove(_expectedPath), Times.Once);
        }

        [Test]
        public void Set_GivenKeyAndValueAndExpiration_ShouldCallRemoveOnIoAdapter()
        {
            _localStorageCacheProvider.Set(_key, "value", default(TimeSpan));

            _mockIoAdapter.Verify(e => e.Remove(_expectedPath), Times.Once);
        }

        [Test]
        public void Set_GivenKeyAndValueAndExpiration_ShouldCallAddOnIoAdapter()
        {
            var expectedValue = "value".ToJson();

            _localStorageCacheProvider.Set(_key, "value", default(TimeSpan));

            _mockIoAdapter.Verify(e => e.Add(BasePath, _key, expectedValue), Times.Once);
        }

        [Test]
        public void Exists_GivenKey_ShouldCallExistsOnIoAdapter()
        {
            _localStorageCacheProvider.Exists(_key);

            _mockIoAdapter.Verify(e => e.Exists(_expectedPath), Times.Once);
        }

        [Test]
        public void Retrieve_GivenKey_ShouldCallRetrieveOnIoAdapter()
        {
            _localStorageCacheProvider.Retrieve<string>(_key);

            _mockIoAdapter.Verify(e => e.Retrieve(_expectedPath), Times.Once);
        }

        [Test]
        public void Retrieve_WhenRetrieveKeyThatNotExists_ShouldAddValue()
        {
            Func<string> function = () => "value";

            _mockIoAdapter.Setup(e => e.Retrieve(It.IsAny<string>())).Returns((string)null);

            var expectedValue = "value".ToJson();

            _localStorageCacheProvider.RetrieveOrElse(_key, default(TimeSpan), function);

            _mockIoAdapter.Verify(e => e.Add(BasePath, _key, expectedValue), Times.Once);
        }

        [Test]
        public void CreateFilePath_GivenKey_ShouldReturnExpectedFilePath()
        {
            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .CreateFilePath(_key)
                .Should()
                .Be(_expectedPath);
        }

        [TestCase("")]
        [TestCase(null)]
        public void IsExpirationValid_GivenNullOrEmptyExpirationRetrieved_ShouldReturnTrue(string expirationRetrieved)
        {
            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .IsExpirationValid(expirationRetrieved)
                .Should()
                .BeTrue();
        }

        [Test]
        public void IsExpirationValid_GivenValidExpirationTime_ShouldReturnTrue()
        {
            const string expirationRetrieved = "01/02/2015 00:00:00";

            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .IsExpirationValid(expirationRetrieved)
                .Should()
                .BeTrue();
        }

        [Test]
        public void IsExpirationValid_GivenExpiredTime_ShouldReturnFalse()
        {
            const string expirationRetrieved = "12/31/2014 00:00:00";

            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .IsExpirationValid(expirationRetrieved)
                .Should()
                .BeFalse();
        }

        [Test]
        public void IsExpirationValid_GivenUnformattedTime_ShouldReturnFalse()
        {
            const string unformattedTime = "01-01-2000 11:00:00";

            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .IsExpirationValid(unformattedTime)
                .Should()
                .BeFalse();
        }

        [Test]
        public void CreateDateToExpire_GivenTimeSpan_ShouldCreateWithExpectedFormat()
        {
            var expiration = TimeSpan.FromHours(2);

            _localStorageCacheProvider
                .AsType<LocalStorageCacheProvider>()
                .CreateDateToExpire(expiration)
                .Should()
                .Be("01/01/2015 14:30:00");
        }
    }
}