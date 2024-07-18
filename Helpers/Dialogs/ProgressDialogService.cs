using CSVReaderTask.Helpers.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVReaderTask.Helpers.Dialogs
{
    public class ProgressDialogService : IProgressDialogService
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ProgressDialogController? _controller = null;

        public ProgressDialogService(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        public async Task ShowProgressAsync(object context, string title, string message)
        {
            if (_controller != null && _controller.IsOpen)
                throw new InvalidOperationException("Menu is already opened");
            _controller = await _dialogCoordinator.ShowProgressAsync(context, title, message);
            
        }

        public async Task HideProgressAsync()
        {
            if (_controller != null && _controller.IsOpen)
            {
                await _controller.CloseAsync();
                _controller = null;
            }   
        }
    }
}
