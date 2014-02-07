using System.IO;
using CreateFlatFileWebsiteFromRazor.Logic;

namespace CreateFlatFileWebsiteFromRazor
{
    internal class FileSystemUploader : IUploader
    {
        private readonly string _rootDirectory;
        private const string Extension = ".html";

        public FileSystemUploader(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public void SaveContentToLocation(string content, string location)
        {
            File.WriteAllText(string.Format("{0}/{1}{2}", _rootDirectory, location, Extension), content);
        }
    }
}