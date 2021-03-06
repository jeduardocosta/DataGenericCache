﻿using DataGenericCache.Exceptions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Exceptions
{
    [TestFixture]
    public class CacheProviderConnectionExceptionTest
    {
        [Test]
        [ExpectedException(typeof(CacheProviderConnectionException))]
        public void Should_ThrowCacheProviderConnectionException()
        {
            throw new CacheProviderConnectionException();
        }

        [Test]
        public void Should_ThrowCacheProviderConnectionException_WithCustomErrorMessage()
        {
            const string exceptionMessage = "ShouldThrowCacheProviderConnectionExceptionWithCustomErrorMessage";

            try
            {
                throw new CacheProviderConnectionException(exceptionMessage);
            }
            catch (CacheProviderConnectionException cacheClientException)
            {
                Assert.AreEqual(exceptionMessage, cacheClientException.Message);
            }
        }
    }
}