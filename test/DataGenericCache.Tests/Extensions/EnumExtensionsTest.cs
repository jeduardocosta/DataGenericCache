using DataGenericCache.Extensions;
using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Extensions
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        [Test]
        public void Should_GetLocalMemoryName_FromEnumObject()
        {
            CacheProvider.LocalMemory
                .GetName()
                .Should()
                .Be("LocalMemory");
        }

        [Test]
        public void Should_GetRedisName_FromEnumObject()
        {
            CacheProvider.Redis
                .GetName()
                .Should()
                .Be("Redis");
        }
    }
}