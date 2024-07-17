namespace CSVReaderTask.Helpers.Interfaces
{
    public interface ILocalizationService
    {
        string GetString(string key);
        void ChangeCulture(string cultureName);
    }
}
