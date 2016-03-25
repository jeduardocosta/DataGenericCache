namespace DataGenericCache.Adapters
{
    public interface IConfigurationAdapter
    {
        string Get(string keyName);

        T GetSections<T>(string keyName);
    }
}