using System;

namespace DataGenericCache.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetName(this Enum enumObject)
        {
            return Enum.GetName(enumObject.GetType(), enumObject);
        }
    }
}