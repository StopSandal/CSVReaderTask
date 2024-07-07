
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
    }
}
