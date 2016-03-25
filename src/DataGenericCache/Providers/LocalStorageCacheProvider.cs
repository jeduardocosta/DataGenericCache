using System;
using System.Globalization;
using DataGenericCache.Adapters;
using DataGenericCache.Extensions;
using DataGenericCache.Settings;

namespace DataGenericCache.Providers
{
    public class LocalStorageCacheProvider : ICacheProvider
    {
        internal string Address { get; private set; }

        private readonly IFileIoAdapter _ioAdapter;

        public LocalStorageCacheProvider()
            : this(new FileIoAdapter())
        {
        }

        internal LocalStorageCacheProvider(IFileIoAdapter ioAdapter)
        {
            _ioAdapter = ioAdapter;
        }

        public void SetupConfiguration(ServerSettings serverSettings)
        {
            Address = serverSettings.Address;
        }

        public void Add<T>(string key, T value, TimeSpan expiration)
        {
            _ioAdapter.Add(Address, key, value.ToJson());

            var expirationKey = CreateFileExpiration(key);
            var dateToExpire = CreateDateToExpire(expiration);

            _ioAdapter.Add(Address, expirationKey, dateToExpire);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var filePath = CreateFilePath(key);
            Remove(filePath);
            Add(key, value, expiration);
        }

        public void Remove(string key)
        {
            var filePath = CreateFilePath(key);
            var expirationKey = CreateFileExpiration(key);
            var expirationFilePath = CreateFilePath(expirationKey);

            _ioAdapter.Remove(filePath);
            _ioAdapter.Remove(expirationFilePath);
        }

        public bool Exists(string key)
        {
            var filePath = CreateFilePath(key);
            return _ioAdapter.Exists(filePath);
        }

        public T Retrieve<T>(string key)
        {
            var filePath = CreateFilePath(key);
            var fileExpirationPath = CreateFilePath(CreateFileExpiration(key));
            var expirationRetrieved = _ioAdapter.Retrieve(fileExpirationPath);

            if (!IsExpirationValid(expirationRetrieved))
            {
                Remove(filePath);
                return default(T);
            }
            
            var retrieved = _ioAdapter.Retrieve(filePath).FromJson<T>();
            return retrieved;
        }
        

        public T RetrieveOrElse<T>(string key, TimeSpan expiration, Func<T> retrievalDelegate)
        {
            var cachedObject = Retrieve<T>(key);

            if (cachedObject == null)
            {
                var retrievedObject = retrievalDelegate();
                Add(key, retrievedObject, expiration);
                cachedObject = retrievedObject;
            }

            return cachedObject;
        }

        internal string CreateFilePath(string key)
        {
            return Address.Combine(key);
        }

        internal bool IsExpirationValid(string expirationRetrieved)
        {
            if (string.IsNullOrWhiteSpace(expirationRetrieved))
            {
                return true;
            }

            DateTime expirationDate;
            DateTime.TryParseExact(expirationRetrieved, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate);
            return Clock.Now() <= expirationDate;
        }

        internal string CreateDateToExpire(TimeSpan expiration)
        {
            return Clock.Now()
                .AddSeconds(expiration.TotalSeconds)
                .ToString(CultureInfo.InvariantCulture);
        }

        private string CreateFileExpiration(string key)
        {
            return $"{key}-expiration";
        }
    }
}