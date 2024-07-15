using CSVReaderTask.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers
{
    internal class LocalizationService : ILocalizationService
    {
        private readonly ResourceManager _resourceManager;

        public LocalizationService()
        {
            _resourceManager = new ResourceManager("CSVReaderTask.Resources.Localization.Strings", typeof(LocalizationService).Assembly);
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }

        public void ChangeCulture(string cultureName)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            CultureInfo.CurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
        }
    }
}
