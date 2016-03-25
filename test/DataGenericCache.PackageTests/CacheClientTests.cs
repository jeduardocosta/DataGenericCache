using System;
using System.Threading;
using DataGenericCache.Providers;
using DataGenericCache.Providers.Clients;
using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.PackageTests
{
    [TestFixture]
    public class CacheClientTests
    {
        private ICacheProvider _cacheProvider;

        [SetUp]
        public void SetUp()
        {
            _cacheProvider = new CacheClient();
        }

        [Test]
        public void Should_RetrieveNull_WhenValueDoesntExist_InLocalMemoryCacher()
        {
            _cacheProvider
                .Retrieve<string>("ShouldRetrieveNullWhenValueDoesntExistKey")
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_Add_AndRetrieveSuccessfull_InLocalMemoryCacher()
        {
            const string key = "ShouldAddAndRetrieveSuccessfullyKey";
            const string value = "ShouldAddAndRetrieveSuccessfullyValue";

            _cacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _cacheProvider
                .Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Add_WhenInvokingRetrieveOrElse_InLocalMemoryCacher()
        {
            const string key = "ShouldAddWhenInvokingRetrieveOrElseKey";
            const string value = "ShouldAddWhenInvokingRetrieveOrElseValue";

            _cacheProvider.RetrieveOrElse(key, TimeSpan.FromSeconds(1), () => value);

            _cacheProvider
                .Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Expire_WhenAdd_WithTimeSpan_InLocalMemoryCacher()
        {
            const string key = "ShouldExpireWhenAddWithTimespanKey";
            const string value = "ShouldExpireWhenAddWithTimespanValue";

            _cacheProvider.Add(key, value, TimeSpan.FromMilliseconds(3));

            Thread.Sleep(5);

            _cacheProvider
                .Retrieve<string>(key)
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_CheckIfValueExists_InLocalMemoryCacher()
        {
            const string key = "ShouldCheckIfValueExistsInLocalMemoryCacher";
            const string value = "ShouldCheckIfValueExistsInLocalMemoryCacher";

            _cacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _cacheProvider
                .Exists(key)
                .Should()
                .BeTrue();

            _cacheProvider.Remove(key);
        }

        [Test]
        public void Should_RemoveValue_InLocalMemoryCacher()
        {
            const string key = "ShouldRemoveValueInLocalMemoryCacher";
            const string value = "ShouldRemoveValueInLocalMemoryCacher";

            _cacheProvider.Add(key, value, TimeSpan.FromSeconds(1));
            _cacheProvider.Remove(key);

            _cacheProvider
                .Exists(key)
                .Should()
                .BeFalse();
        }

        [Test]
        public void Should_SetupConfiguration_InLocalMemoryCacher()
        {
            _cacheProvider.SetupConfiguration(new ServerSettings());
        }
    }
}