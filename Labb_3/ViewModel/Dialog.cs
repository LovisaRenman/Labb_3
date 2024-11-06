using System.Windows;
using Labb_3.Command;
using Labb_3.Dialogs;

namespace Labb_3.ViewModel
{
    public interface IDialogService
    {
        public void ShowDialog(object mainWindowViewModel);
        public void CloseDialog();
    }
    class PackOptionsDialogService : IDialogService
    {
        private Window _dialog;
        public void CloseDialog()
        {
            _dialog?.Close();
        }

        public void ShowDialog(object mainWindowViewModel)
        {
            _dialog = new PackOptionsDialog();
            _dialog.DataContext = mainWindowViewModel;
            _dialog.Show();
        }
    }

    class CreateNewPackDialogService : IDialogService
    {
        private Window _dialog;
        public void CloseDialog()
        {
            _dialog?.Close();
        }

        public void ShowDialog(object mainWindowViewModel)
        {
            _dialog = new CreateNewPackDialog();
            _dialog.DataContext = mainWindowViewModel;
            _dialog.Show();
        }
    }

    class ImportQuestionsDialogService : IDialogService
    {
        private Window _dialog;

        public DelegateCommand CancelCommand { get; }

        public void CloseDialog()
        {
            _dialog?.Close();
        }

        public void ShowDialog(object mainWindowViewModel)
        {
            _dialog = new ImportQuestionsDialog();
            _dialog.DataContext = mainWindowViewModel;
            _dialog.Show();
        }
    }
}
