using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class OpenDialog : IOpenDialog
    {
        public string? ShowOpenDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter
            };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }
    }
}
