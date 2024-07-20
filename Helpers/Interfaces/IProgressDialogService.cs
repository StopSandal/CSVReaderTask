using MahApps.Metro.Controls.Dialogs;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Provides functionality to display and manage progress dialogs.
    /// </summary>
    public interface IProgressDialogService
    {
        /// <summary>
        /// Displays a progress dialog with the specified title and message.
        /// </summary>
        /// <param name="context">The context in which the dialog is displayed.</param>
        /// <param name="title">The title of the progress dialog.</param>
        /// <param name="message">The message displayed in the progress dialog.</param>
        /// <returns>A task representing the asynchronous operation, with the task result containing the progress dialog controller.</returns>
        Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message);

        /// <summary>
        /// Hides the specified progress dialog if it is open.
        /// </summary>
        /// <param name="menu">The progress dialog controller to be hidden.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task HideProgressAsync(ProgressDialogController menu);
    }
}
