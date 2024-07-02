using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Win32;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class SaveDialog : ISaveDialog
    {
        public string? ShowSaveDialog(string filter, string defaultExt, string title)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = title
            };
            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
    }
}
