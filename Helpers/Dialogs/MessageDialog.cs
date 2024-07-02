using CSVReaderTask.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class MessageDialog : IMessageDialog
    {
        private const string ErrorTitle = "Error";
        public void ShowError(string message)
        {
            MessageBox.Show(message, ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowOK(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
