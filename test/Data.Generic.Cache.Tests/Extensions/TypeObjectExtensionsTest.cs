﻿using Data.Generic.Cache.Extensions;
using Data.Generic.Cache.Providers;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Extensions
{
    [TestFixture]
    public class TypeObjectExtensionsTest
    {
        private const string JsonSample = @"{""Address"":""server-value"",""Port"":1234,""Password"":""pass""}";

        [Test]
        public void ToJson_GivenObject_ShouldConvertToExpectedJson()
        {
            new ServerSettings("server-value", 1234, "pass")
                .ToJson()
                .Should()
                .Be(JsonSample);
        }

        [Test]
        public void FromJson_GivenJson_ShouldConvertToExpectedObject()
        {
            JsonSample
                .FromJson<ServerSettings>()
                .ShouldBeEquivalentTo(new ServerSettings("server-value", 1234, "pass"));
        }

        [Test]
        public void AsType_GivenInterfaceType_ShouldReturnExpectedDerivedClass()
        {
            ICacheProvider redisCacheProvider = new RedisCacheProvider();

            redisCacheProvider
                .AsType<RedisCacheProvider>()
                .Should()
                .BeOfType<RedisCacheProvider>();
        }
    }
}