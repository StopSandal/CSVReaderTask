using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IFileExport
    {
        Task ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection);
    }
}
