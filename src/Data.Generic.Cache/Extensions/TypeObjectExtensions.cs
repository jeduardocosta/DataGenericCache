using Data.Generic.Cache.Parsers;
using StackExchange.Redis;

namespace Data.Generic.Cache.Extensions
{
    public static class TypeObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var jsonParser = new JsonParser();
            return jsonParser.Parse(obj);
        }

        public static T FromJson<T>(this RedisValue obj)
        {
            var jsonParser = new JsonParser();
            return jsonParser.Parse<T>(obj);
        }
    }
}