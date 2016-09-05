using System;

namespace DataGenericCache.Providers.Entities
{
    public class MemoryData<T>
    {
        private readonly DateTime? _timeToExpire;
        public T Value { get; private set; }

        public MemoryData(T value)
            : this(value, null)
        {
        }

        public MemoryData(T value, TimeSpan? expiration)
        {
            Value = value;

            if (expiration.HasValue)
            {
                _timeToExpire = Clock.Now().Add(expiration.Value);
            }
        }

        public bool IsExpired()
        {
            return _timeToExpire.HasValue && Clock.Now() > _timeToExpire;
        }
    }
}