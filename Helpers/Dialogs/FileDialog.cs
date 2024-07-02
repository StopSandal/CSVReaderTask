using CSVReaderTask.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class FileDialog : IFileDialog
    {
        private readonly IOpenDialog _openDialog;
        private readonly ISaveDialog _saveDialog;

        public FileDialog(IOpenDialog openDialog, ISaveDialog saveDialog)
        {
            _openDialog = openDialog;
            _saveDialog = saveDialog;
        }

        public string? ShowOpenDialog(string filter)
        {
            return _openDialog.ShowOpenDialog(filter);
        }

        public string? ShowSaveDialog(string filter, string defaultExt, string title)
        {
            return _saveDialog.ShowSaveDialog(filter, defaultExt, title);
        }
    }
}
