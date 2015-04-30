using Newtonsoft.Json;

namespace Data.Generic.Cache.Parsers
{
    internal interface IJsonParser
    {
        T Parse<T>(string data);
        string Parse<T>(T data);
    }

    internal class JsonParser : IJsonParser
    {
        public T Parse<T>(string data)
        {
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