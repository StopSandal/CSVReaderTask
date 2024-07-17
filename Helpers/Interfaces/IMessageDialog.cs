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
    }
}
