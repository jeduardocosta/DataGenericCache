using Data.Generic.Cache.Extensions;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Extensions
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