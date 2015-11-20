using Newtonsoft.Json;

namespace Data.Generic.Cache.Parsers
{
    internal class JsonParser : IJsonParser
    {
        public T Parse<T>(string data)
        {
            if (data == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        public string Parse<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}