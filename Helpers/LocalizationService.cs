using CSVReaderTask.Helpers.Interfaces;
using System.Globalization;
using System.Resources;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Implementation of <see cref="ILocalizationService"/> for retrieving localized strings and changing the application's culture.
    /// </summary>
    internal class LocalizationService : ILocalizationService
    {
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationService"/> class.
        /// </summary>
        public LocalizationService()
        {
            _resourceManager = new ResourceManager("CSVReaderTask.Resources.Localization.Strings", typeof(LocalizationService).Assembly);
        }

        /// <inheritdoc/>
        public string GetString(string key)
        {
            return _resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }

        /// <inheritdoc/>
        public void ChangeCulture(string cultureName)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
        }
    }
}
