using CreateFlatFileWebsiteFromRazor.Logic;

namespace CreateFlatFileWebsiteFromRazor
{
    static class Program
    {
        static void Main()
        {
            const string workingRoot = "../../files";
            IContentRepository content = new FileSystemContentRepository(workingRoot + "/content");
            IDataRepository data = new FileSystemDataRepository(workingRoot + "/data");
            IUploader uploader = new FileSystemUploader(workingRoot + "/output");

            var productIds = new[] {"1", "2", "3", "4", "5"};
            var renderer = new RenderHtmlPage(content, data);
            
            foreach (var productId in  productIds)
            {
                var result = renderer.BuildContentResult("product", productId);
                uploader.SaveContentToLocation(result, productId);
            }
        }
    }
}
