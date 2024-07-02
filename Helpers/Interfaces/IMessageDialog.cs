using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IMessageDialog
    {
        void ShowMessage(string message, string title);
        void ShowOK(string message, string title);
        void ShowError(string message);
    }
}
