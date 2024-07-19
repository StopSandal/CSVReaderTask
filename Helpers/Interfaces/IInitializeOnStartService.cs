
namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Defines a service responsible for performing initialization tasks when the application starts.
    /// </summary>
    public interface IInitializeOnStartService
    {
        /// <summary>
        /// Performs the initialization logic.
        /// </summary>
        /// <returns>True if initialization was successful, false otherwise.</returns>
        bool Initialize();
        /// <summary>
        /// Sets a new connection string for the database context and reinitializes the context.
        /// </summary>
        /// <param name="connectionString">The new connection string.</param>
        public void SetNewConnectionString(string connectionString);
    }
}
