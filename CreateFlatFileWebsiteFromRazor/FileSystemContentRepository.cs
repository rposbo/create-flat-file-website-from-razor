using System.IO;
using CreateFlatFileWebsiteFromRazor.Logic;

namespace CreateFlatFileWebsiteFromRazor
{
    internal class FileSystemContentRepository : IContentRepository
    {
        private readonly string _rootDirectory;
        private const string Extension = ".cshtml";

        public FileSystemContentRepository(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public string GetContent(string id)
        {
            var result = File.ReadAllText(string.Format("{0}/{1}{2}", _rootDirectory, id, Extension));
            return result;
        }
    }
}