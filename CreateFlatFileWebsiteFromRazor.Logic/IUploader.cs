namespace CreateFlatFileWebsiteFromRazor.Logic
{
    public interface IUploader
    {
        void SaveContentToLocation(string content, string location);
    }
}