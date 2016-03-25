namespace DataGenericCache.Adapters
{
    public interface IFileIoAdapter
    {
        void Add(string path, string key, string value);
        void Remove(string path);
        string Retrieve(string path);
        bool Exists(string path);
        void Create(string path);
    }
}