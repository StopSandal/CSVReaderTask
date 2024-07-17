namespace CSVReaderTask.Helpers.Interfaces
{
    /// <summary>
    /// Interface defining a method to show an save file dialog.
    /// </summary>
    public interface ISaveDialog
    {
        /// <summary>
        /// Shows a save file dialog and returns the selected file path.
        /// </summary>
        /// <param name="filter">The filter string specifying the types of files to display.</param>
        /// <param name="defaultExt">The default file extension to use if the user does not specify one.</param>
        /// <param name="title">The title of the save file dialog.</param>
        /// <returns>The selected file path, or null if no file path was selected.</returns>
        string? ShowSaveDialog(string filter, string defaultExt, string title);
    }
}
