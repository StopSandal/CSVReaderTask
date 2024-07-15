using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface ILocalizationService
    {
        string GetString(string key);
        void ChangeCulture(string cultureName);
    }
}
