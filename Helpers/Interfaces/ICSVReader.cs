using CSVReaderTask.Models;

namespace CSVReaderTask.Helpers.Interfaces
{

    /// <summary>
    /// Provides methods to read a CSV file and save its contents to the database.
    /// </summary>
    public interface ICSVReader
    {
        /// <summary>
        /// Reads a CSV file from the specified path and saves its contents to the database.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<IEnumerable<Person>> ReadFilePersonAsync(string filePath);
    }
}
