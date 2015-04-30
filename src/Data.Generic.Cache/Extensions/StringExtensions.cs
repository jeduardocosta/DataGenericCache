using System;

namespace Data.Generic.Cache.Extensions
{
    public static class StringExtensions
    {
        public static T ParseToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}