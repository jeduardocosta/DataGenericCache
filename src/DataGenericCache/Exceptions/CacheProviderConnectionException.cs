using System;

namespace DataGenericCache.Exceptions
{
    [Serializable]
    public class CacheProviderConnectionException : Exception
    {
        public CacheProviderConnectionException() { }

        public CacheProviderConnectionException(string message) : base(message) { }
    }
}