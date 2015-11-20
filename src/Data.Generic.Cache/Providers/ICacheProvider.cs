using System;
using Data.Generic.Cache.Settings;

namespace Data.Generic.Cache.Providers
{
    public interface ICacheProvider
    {
        void Add<T>(string key, T value, TimeSpan expiration);
        void Set<T>(string key, T value, TimeSpan expiratio);
        void Remove(string key);
        bool Exists(string key);

        T Retrieve<T>(string key);
        T RetrieveOrElse<T>(string key, TimeSpan expiration, Func < T> retrievalDelegate);

        void SetupConfiguration(ServerSettings serverSettings);
    }
}