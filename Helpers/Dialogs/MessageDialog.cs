using CSVReaderTask.Helpers.Interfaces;
using System.Windows;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of IMessageDialog interface using MessageBox to show message dialogs.
    /// </summary>
    public class MessageDialog : IMessageDialog
    {
        private const string ErrorTitle = "Error";

        /// <inheritdoc/>
        public void ShowError(string message)
        {
            MessageBox.Show(message, ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <inheritdoc/>
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <inheritdoc/>
        public void ShowOK(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
