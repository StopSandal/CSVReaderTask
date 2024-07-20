using CSVReaderTask.Helpers.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class ProgressDialogService : IProgressDialogService
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public ProgressDialogService(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        public async Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message)
        {
            return await _dialogCoordinator.ShowProgressAsync(context, title, message);
        }

        public async Task HideProgressAsync(ProgressDialogController menu)
        {
            if (menu != null && menu.IsOpen)
            {
                await menu.CloseAsync();
            }
        }
    }
}
