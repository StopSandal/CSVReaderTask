using CSVReaderTask.Helpers.Interfaces;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of IFileDialog interface that combines open and save file dialog functionalities.
    /// </summary>
    public class FileDialog : IFileDialog
    {
        private readonly IOpenDialog _openDialog;
        private readonly ISaveDialog _saveDialog;

        /// <summary>
        /// Initializes a new instance of the FileDialog class.
        /// </summary>
        /// <param name="openDialog">The implementation of IOpenDialog used for opening files.</param>
        /// <param name="saveDialog">The implementation of ISaveDialog used for saving files.</param>
        public FileDialog(IOpenDialog openDialog, ISaveDialog saveDialog)
        {
            _openDialog = openDialog;
            _saveDialog = saveDialog;
        }

        /// <inheritdoc />
        public string? ShowOpenDialog(string filter)
        {
            return _openDialog.ShowOpenDialog(filter);
        }

        /// <inheritdoc />
        public string? ShowSaveDialog(string filter, string defaultExt, string title)
        {
            return _saveDialog.ShowSaveDialog(filter, defaultExt, title);
        }
    }
}
