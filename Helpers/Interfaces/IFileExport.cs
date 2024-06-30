using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Provides methods to export data to a file.
    /// </summary>
    public interface IFileExport
    {
        /// <summary>
        /// Exports the specified collection of data to a file asynchronously.
        /// </summary>
        /// <typeparam name="TClass">The type of objects in the data collection.</typeparam>
        /// <param name="filePath">The path where the file will be saved.</param>
        /// <param name="dataCollection">The collection of data to export.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExportFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection);
    }
}
