using DataGenericCache.Parsers;

namespace DataGenericCache.Extensions
{
    public static class TypeObjectExtensions
    {
        private static readonly JsonParser JsonParser = new JsonParser();

        public static string ToJson(this object obj)
        {
            return JsonParser.Parse(obj);
        }

        public static T FromJson<T>(this string obj)
        {
            return JsonParser.Parse<T>(obj);
        }

        public static T AsType<T>(this object obj)
        {
            return (T)obj;
        }
    }
}