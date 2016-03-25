using System;
using System.Threading;
using DataGenericCache.Providers;
using DataGenericCache.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers
{
    [TestFixture]
    public class LocalMemoryCacheProviderTest
    {
        private ICacheProvider _localMemoryCacheProvider;

        [SetUp]
        public void SetUp()
        {
            _localMemoryCacheProvider = new LocalMemoryCacheProvider();
        }

        [Test]
        public void Should_RetrieveNull_WhenValueDoesntExist_InLocalMemoryCacher()
        {
            _localMemoryCacheProvider.Retrieve<string>("ShouldRetrieveNullWhenValueDoesntExistKey")
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_Add_AndRetrieveSuccessfull_InLocalMemoryCacher()
        {
            const string key = "ShouldAddAndRetrieveSuccessfullyKey";
            const string value = "ShouldAddAndRetrieveSuccessfullyValue";

            _localMemoryCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _localMemoryCacheProvider
                .Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Add_WhenInvokingRetrieveOrElse_InLocalMemoryCacher()
        {
            const string key = "ShouldAddWhenInvokingRetrieveOrElseKey";
            const string value = "ShouldAddWhenInvokingRetrieveOrElseValue";

            _localMemoryCacheProvider.RetrieveOrElse(key, TimeSpan.FromSeconds(1), () => value);

            _localMemoryCacheProvider
                .Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Expire_WhenAdd_WithTimeSpan_InLocalMemoryCacher()
        {
            const string key = "ShouldExpireWhenAddWithTimespanKey";
            const string value = "ShouldExpireWhenAddWithTimespanValue";

            _localMemoryCacheProvider.Add(key, value, TimeSpan.FromMilliseconds(3));

            Thread.Sleep(5);

            _localMemoryCacheProvider
                .Retrieve<string>(key)
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_CheckIfValueExists_InLocalMemoryCacher()
        {
            const string key = "ShouldCheckIfValueExistsInLocalMemoryCacher";
            const string value = "ShouldCheckIfValueExistsInLocalMemoryCacher";

            _localMemoryCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _localMemoryCacheProvider
                .Exists(key)
                .Should()
                .BeTrue();

            _localMemoryCacheProvider.Remove(key);
        }

        [Test]
        public void Should_RemoveValue_InLocalMemoryCacher()
        {
            const string key = "ShouldRemoveValueInLocalMemoryCacher";
            const string value = "ShouldRemoveValueInLocalMemoryCacher";

            _localMemoryCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));
            _localMemoryCacheProvider.Remove(key);

            _localMemoryCacheProvider
                .Exists(key)
                .Should()
                .BeFalse();
        }

        [Test]
        public void Should_SetupConfiguration_InLocalMemoryCacher()
        {
            _localMemoryCacheProvider.SetupConfiguration(It.IsAny<ServerSettings>());
        }
    }
}