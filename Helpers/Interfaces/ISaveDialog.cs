using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    public interface ISaveDialog
    {
        string? ShowSaveDialog(string filter, string defaultExt, string title);
    }
}
