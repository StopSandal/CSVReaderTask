using CSVReaderTask.Models;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Interface defining asynchronous operations for saving persons to a database.
    /// </summary>
    public interface IDBPersonSaveAsync
    {
        /// <summary>
        /// Asynchronously saves a collection of persons to the database.
        /// </summary>
        /// <param name="peoples">The collection of persons to save.</param>
        /// <returns>A task representing the asynchronous operation. The task result is the count of records saved.</returns>
        Task<int> SavePersonsToDBAsync(IEnumerable<Person> peoples);
    }
}
