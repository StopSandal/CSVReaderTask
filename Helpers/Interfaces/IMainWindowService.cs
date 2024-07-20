using CSVReaderTask.Models;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Provides methods for interacting with the main window of the application.
    /// </summary>
    public interface IMainWindowService
    {
        /// <summary>
        /// Reads a CSV file from the specified path asynchronously.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A task representing the asynchronous operation with count of added records.</returns>
        Task<int> ReadCSVFileAsync(string filePath);

        /// <summary>
        /// Saves person information to an Excel file asynchronously.
        /// </summary>
        /// <param name="filePath">The path where the Excel file will be saved.</param>
        /// <param name="dataCollection">The collection of person data to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SavePersonInfoToExcelAsync(string filePath, IAsyncEnumerable<Person> dataCollection);

        /// <summary>
        /// Saves person information to an XML file asynchronously.
        /// </summary>
        /// <param name="filePath">The path where the XML file will be saved.</param>
        /// <param name="dataCollection">The collection of person data to save.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SavePersonInfoToXMLAsync(string filePath, IAsyncEnumerable<Person> dataCollection);
    }
}
