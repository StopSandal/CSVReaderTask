namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Provides localization services, such as retrieving localized strings and changing the application's culture.
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Retrieves the localized string for the specified key.
        /// </summary>
        /// <param name="key">The key of the localized string to retrieve.</param>
        /// <returns>The localized string associated with the specified key.</returns>
        string GetString(string key);

        /// <summary>
        /// Changes the application's culture to the specified culture name.
        /// </summary>
        /// <param name="cultureName">The name of the culture to change to (e.g., "en-US", "ru-RU").</param>
        void ChangeCulture(string cultureName);
    }
}
