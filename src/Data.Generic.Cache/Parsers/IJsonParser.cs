namespace Data.Generic.Cache.Parsers
{
    internal interface IJsonParser
    {
        T Parse<T>(string data);
        string Parse<T>(T data);
    }
}