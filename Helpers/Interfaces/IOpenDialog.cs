using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Interface defining a method to show an open file dialog.
    /// </summary>
    public interface IOpenDialog
    {
        /// <summary>
        /// Shows an open file dialog and returns the selected file path.
        /// </summary>
        /// <param name="filter">The filter string specifying the types of files to display.</param>
        /// <returns>The selected file path, or null if no file was selected.</returns>
        string? ShowOpenDialog(string filter);
    }
}
