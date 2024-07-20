using CSVReaderTask.Helpers.Interfaces;
using MahApps.Metro.Controls.Dialogs;

namespace CSVReaderTask.Helpers.Dialogs
{
    /// <summary>
    /// Implementation of <see cref="IProgressDialogService"/> using MahApps.Metro for displaying progress dialogs.
    /// </summary>
    public class ProgressDialogService : IProgressDialogService
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialogService"/> class.
        /// </summary>
        /// <param name="dialogCoordinator">The dialog coordinator for displaying dialogs.</param>
        public ProgressDialogService(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        /// <inheritdoc/>
        public async Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message)
        {
            return await _dialogCoordinator.ShowProgressAsync(context, title, message);
        }

        /// <inheritdoc/>
        public async Task HideProgressAsync(ProgressDialogController menu)
        {
            if (menu != null && menu.IsOpen)
            {
                await menu.CloseAsync();
            }
        }
    }
}
