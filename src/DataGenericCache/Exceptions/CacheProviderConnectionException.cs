using System;

namespace DataGenericCache.Exceptions
{
    [Serializable]
    internal class CacheProviderConnectionException : Exception
    {
        public CacheProviderConnectionException() { }

        public CacheProviderConnectionException(string message) : base(message) { }
    }
}