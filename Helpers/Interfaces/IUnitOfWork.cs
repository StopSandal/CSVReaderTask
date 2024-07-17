using CSVReaderTask.Models;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Defines the contract for the unit of work, which includes repository properties and a save method.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for <see cref="Person"/> entities.
        /// </summary>
        IRepositoryAsync<Person> PersonRepository { get; }

        /// <summary>
        /// Saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveAsync();
    }
}
