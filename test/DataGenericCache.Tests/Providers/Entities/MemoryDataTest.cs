using System;
using System.Threading;
using DataGenericCache.Providers.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Providers.Entities
{
    [TestFixture]
    public class MemoryDataTest
    {
        [Test]
        public void Value_WhenCreateMemoryDatObject_ShouldReturnExpectedValue()
        {
            new MemoryData<string>("something")
                .Value
                .Should()
                .Be("something");
        }

        [Test]
        public void IsExpired_WhenNotInformExpiration_ShouldBeFalse()
        {
            new MemoryData<string>("something")
                .IsExpired()
                .Should()
                .BeFalse();
        }

        [Test]
        public void IsExpired_WhenInformExpirationAndItIsNotExpired_ShouldBeFalse()
        {
            new MemoryData<string>("something", TimeSpan.FromSeconds(5))
                .IsExpired()
                .Should()
                .BeFalse();
        }

        [Test]
        public void IsExpired_WhenInformExpirationAndItIsExpired_ShouldBeTrue()
        {
            var memoryData = new MemoryData<string>("something", TimeSpan.FromMilliseconds(10));

            Thread.Sleep(20);

            memoryData
                .IsExpired()
                .Should()
                .BeTrue();
        }
    }
}