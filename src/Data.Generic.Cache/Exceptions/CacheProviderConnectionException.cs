using System;

namespace Data.Generic.Cache.Exceptions
{
    [Serializable]
    internal class CacheProviderConnectionException : Exception
    {
        public CacheProviderConnectionException() { }

        public CacheProviderConnectionException(string message) : base(message) { }
    }
}