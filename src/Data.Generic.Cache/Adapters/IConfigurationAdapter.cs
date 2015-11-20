namespace Data.Generic.Cache.Adapters
{
    public interface IConfigurationAdapter
    {
        string Get(string keyName);

        T GetSections<T>(string keyName);
    }
}