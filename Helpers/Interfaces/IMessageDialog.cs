using MahApps.Metro.Controls.Dialogs;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IMessageDialog
    {
        /// <summary>
        /// Shows an error message dialog with a predefined title ("Error").
        /// </summary>
        /// <param name="message">The error message to display.</param>
        void ShowMessage(string message, string title);
        /// <summary>
        /// Shows a message dialog with the specified title and an information icon.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the message dialog.</param>
        void ShowOK(string message, string title);
        /// <summary>
        /// Shows an OK message dialog with the specified title and an information icon.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="title">The title of the message dialog.</param>
        void ShowError(string message);
        /// <summary>
        /// Shows a retry dialog with a message and returns the user's choice.
        /// </summary>
        /// <param name="context">The context in which the dialog is displayed.</param>
        /// <param name="message">The message to display in the dialog.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's choice.</returns>
        public Task<MessageDialogResult> ShowRetryDialog(object context, string message);
        /// <summary>
        /// Shows an input dialog with a title and prompt, and returns the user's input.
        /// </summary>
        /// <param name="context">The context in which the dialog is displayed.</param>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="prompt">The prompt message to display in the dialog.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's input.</returns>
        public Task<string> ShowInputDialog(object context, string title, string prompt);
    }
}
