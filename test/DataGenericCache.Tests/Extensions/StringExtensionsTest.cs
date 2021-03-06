﻿using DataGenericCache.Extensions;
using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void Should_ConvertStringValue_ToRedisEntry_AsCacheProviderEnum()
        {
            "redis"
                .ParseToEnum<CacheProvider>()
                .Should()
                .Be(CacheProvider.Redis);
        }

        [Test]
        public void Should_ConvertStringValue_AndIgnoreCase_ToRedisEntry_AsCacheProviderEnum()
        {
            "REDIS"
                .ParseToEnum<CacheProvider>()
                .Should()
                .Be(CacheProvider.Redis);
        }

        [Test]
        public void Should_ConvertStringValue_ToLocalMemory_AsCacheProviderEnum()
        {
            "localmemory"
                .ParseToEnum<CacheProvider>()
                .Should()
                .Be(CacheProvider.LocalMemory);
        }

        [Test]
        public void Combine_GivenBasePathAndValue_ShouldReturnExpectedValue()
        {
            "c:\\base"
                .Combine("folder")
                .Should()
                .Be("c:\\base\\folder");
        }
    }
}