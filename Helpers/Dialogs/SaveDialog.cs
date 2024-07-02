using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Win32;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of ISaveDialog interface using SaveFileDialog to show a save file dialog.
    /// </summary>
    public class SaveDialog : ISaveDialog
    {
        /// <inheritdoc/>
        public string? ShowSaveDialog(string filter, string defaultExt, string title)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = title
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
    }
}
