using CSVReaderTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Provides methods to export person data to an XML file, inheriting from <see cref="IXMLExport"/>.
    /// </summary>
    public interface IXMLPersonExport : IXMLExport
    {
        /// <summary>
        /// Exports the specified collection of person data to an XML file asynchronously.
        /// </summary>
        /// <typeparam name="TClass">The type of objects in the data collection, must be derived from <see cref="Person"/>.</typeparam>
        /// <param name="filePath">The path where the XML file will be saved.</param>
        /// <param name="dataCollection">The collection of person data to export.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExportPersonsFileAsync<TClass>(string filePath, IEnumerable<TClass> dataCollection) where TClass : Person;
    }
}
