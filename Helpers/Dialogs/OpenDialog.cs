using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Win32;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of IOpenDialog interface using OpenFileDialog to show an open file dialog.
    /// </summary>
    public class OpenDialog : IOpenDialog
    {
        /// <inheritdoc/>
        public string? ShowOpenDialog(string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }
    }
}
