using MahApps.Metro.Controls.Dialogs;


namespace CSVReaderTask.Helpers.Interfaces
{
    public interface IProgressDialogService
    {
        Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message);
        Task HideProgressAsync(ProgressDialogController menu);
    }
}
