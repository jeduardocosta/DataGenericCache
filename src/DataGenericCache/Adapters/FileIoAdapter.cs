using System.IO;

namespace DataGenericCache.Adapters
{
    internal class FileIoAdapter : IFileIoAdapter
    {
        public void Add(string path, string key, string value)
        {
            if (!Exists(path))
            {
                Create(path);
            }

            var fullPath = Path.Combine(path, key);
            File.WriteAllText(fullPath, value);
        }

        public void Remove(string path)
        {
            File.Delete(path);
        }

        public string Retrieve(string path)
        {
            return !Exists(path) ? null : File.ReadAllText(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void Create(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}