using System;
using System.Linq;
using System.Threading;
using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Providers
{
    [TestFixture]
    public class RedisCacheProviderTest
    {
        private ICacheProvider _redisCacheProvider;

        [SetUp]
        public void SetUp()
        {
            var redisProviderSettings = new ProviderSettingsConfig(new ConfigurationAdapter())
                .GetProviders()
                .FirstOrDefault(it => it.Type == CacheProvider.Redis);

            _redisCacheProvider = new RedisCacheProvider();

            if (redisProviderSettings == null)
                throw new Exception("failed to create 'RedisCacheProvider' instance in 'RedisCacheProviderTest'.");

            _redisCacheProvider.SetupConfiguration(redisProviderSettings.ServerSettings);
        }

        [Test]
        public void Should_RetrieveNull_WhenValueDoesntExist_InRedisCacheProvider()
        {
            _redisCacheProvider.Retrieve<string>("ShouldRetrieveNullWhenValueDoesntExistKey")
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_Add_AndRetrieveSuccessfull_InRedisCacheProvider()
        {
            const string key = "ShouldAddAndRetrieveSuccessfullyKey";
            const string value = "ShouldAddAndRetrieveSuccessfullyValue";

            _redisCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _redisCacheProvider.Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Add_WhenInvokingRetrieveOrElse_InRedisCacheProvider()
        {
            const string key = "ShouldAddWhenInvokingRetrieveOrElseKey";
            const string value = "ShouldAddWhenInvokingRetrieveOrElseValue";

            _redisCacheProvider.RetrieveOrElse(key, TimeSpan.FromSeconds(1), () => value);

            _redisCacheProvider.Retrieve<string>(key)
                .Should()
                .Be(value);
        }

        [Test]
        public void Should_Expire_WhenAdd_WithTimeSpan_InRedisCacheProvider()
        {
            const string key = "ShouldExpireWhenAddWithTimespanKey";
            const string value = "ShouldExpireWhenAddWithTimespanValue";

            _redisCacheProvider.Add(key, value, TimeSpan.FromMilliseconds(3));

            Thread.Sleep(5);

            _redisCacheProvider.Retrieve<string>(key)
                .Should()
                .BeNull();
        }

        [Test]
        public void Should_CheckIfValueExists_InRedisCacheProvider()
        {
            const string key = "ShouldCheckIfValueExistsInLocalMemoryCacher";
            const string value = "ShouldCheckIfValueExistsInLocalMemoryCacher";

            _redisCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));

            _redisCacheProvider.Exists(key)
                .Should()
                .BeTrue();

            _redisCacheProvider.Remove(key);
        }

        [Test]
        public void Should_RemoveValue_InRedisCacheProvider()
        {
            const string key = "ShouldRemoveValueInLocalMemoryCacher";
            const string value = "ShouldRemoveValueInLocalMemoryCacher";

            _redisCacheProvider.Add(key, value, TimeSpan.FromSeconds(1));
            _redisCacheProvider.Remove(key);

            _redisCacheProvider.Exists(key)
                .Should()
                .BeFalse();
        }

        [Test]
        public void Should_SetupConfiguration_InRedisCacheProvider()
        {
            new RedisCacheProvider().SetupConfiguration(new ServerSettings("localhost", 6379, string.Empty));
        }

        [Test]
        public void Should_CheckIfProviderIsWorking_InRedisCacheProvide()
        {
            _redisCacheProvider.IsWorking()
                .Should()
                .BeTrue();
        }
    }
}