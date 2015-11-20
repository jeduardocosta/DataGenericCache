using System;
using System.IO;

namespace Data.Generic.Cache.Extensions
{
    public static class StringExtensions
    {
        public static T ParseToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string Combine(this string value, string newValue)
        {
            return Path.Combine(value, newValue);
        }
    }
}