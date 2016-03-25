namespace DataGenericCache.Adapters
{
    internal interface IConfigurationAdapter
    {
        string Get(string keyName);

        T GetSections<T>(string keyName);
    }
}