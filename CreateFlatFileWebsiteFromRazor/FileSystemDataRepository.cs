using System.IO;
using CreateFlatFileWebsiteFromRazor.Logic;

namespace CreateFlatFileWebsiteFromRazor
{
    internal class FileSystemDataRepository : IDataRepository
    {
        private readonly string _rootDirectory;
        private const string Extension = ".json";

        public FileSystemDataRepository(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public string GetData(string id)
        {
            var results = File.ReadAllText(string.Format("{0}/{1}{2}", _rootDirectory, id, Extension));
            return results;
        }
    }
}