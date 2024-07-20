using CSVReaderTask.Helpers.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of IMessageDialog interface using MessageBox to show message dialogs.
    /// </summary>
    public class MessageDialog : IMessageDialog
    {
        private const string ErrorTitle = "Error";
        private readonly IDialogCoordinator _dialogCoordinator;

        public MessageDialog(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

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
        /// <inheritdoc/>
        public async Task<MessageDialogResult> ShowRetryDialog(object context, string message)
        {
            return await _dialogCoordinator.ShowMessageAsync(
                context,
                ErrorTitle,
                message,
                MessageDialogStyle.AffirmativeAndNegative
            );
        }
        /// <inheritdoc/>
        public async Task<string> ShowInputDialog(object context, string title, string prompt)
        {
            var answer = await _dialogCoordinator.ShowInputAsync(context, title, prompt);

            return answer;
        }
    }
}
